using Bootcamp2_AspMVC.interfaces;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CheckoutService> _logger;

        public  CheckoutService(IUnitOfWork uow, ILogger<CheckoutService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<(bool ok, string message, int? orderId)> CreateOrderFromCartAsync(int customerId, TimeSpan? reservationDuration = null)
        {
            reservationDuration ??= TimeSpan.FromMinutes(30);

            var cart = await _uow.CartItemsRepository.GetByCustomerWithProductAsync(customerId);
            if (!cart.Any())
                return (false, "السلة فارغة.", null);

            // تحقق التوفر
            foreach (var ci in cart)
            {
                var available = (ci.Product?.Qty ?? 0) - (ci.Product?.ReservedQty ?? 0);
                if (ci.Quantity <= 0)
                    return (false, $"الكمية غير صالحة للمنتج {ci.Product?.ProductName}.", null);

                if (ci.Quantity > available)
                    return (false, $"لا توجد كمية كافية من المنتج {ci.Product?.ProductName}. المتاح: {available}.", null);
            }

            await using var tx = await _uow.BeginTransactionAsync();
            try
            {
                // حجز الكميات
                foreach (var ci in cart)
                {
                    ci.Product!.ReservedQty += ci.Quantity; // حجز مؤقت
                    _uow.Products.Update(ci.Product);
                }

                var now = DateTime.UtcNow;
                var order = new Order
                {
                    CustomerId = customerId,
                    OrderNumber = $"ORD-{now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
                    Status = OrderStatus.Pending,
                    CreatedAt = now,
                    ReservationExpiresAt = now.Add(reservationDuration.Value),
                    Discount = 0m,
                    Shipping = 0m
                };

                decimal subtotal = 0m;
                foreach (var ci in cart)
                {
                    var item = new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.Product!.Price
                    };
                    subtotal += item.UnitPrice * item.Quantity;
                    order.Items.Add(item);
                }
                order.Subtotal = subtotal;

                await _uow.OrdersRepository.AddAsync(order);
                //------------------------------------------------------
                // تفريغ السلة
                _uow.CartItemsRepository.RemoveRange(cart);

                await _uow.SaveChangesAsync();
                await tx.CommitAsync();

                return (true, "تم إنشاء الطلب وحجز المخزون بنجاح.", order.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency while reserving stock.");
                await tx.RollbackAsync();
                return (false, "حدث تعارض أثناء الحجز. حاول مرة أخرى.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order.");
                await tx.RollbackAsync();
                return (false, "حدث خطأ أثناء إنشاء الطلب.", null);
            }
        }

        public async Task<int> MarkOrderPaidAsync(int orderId)
        {
            await using var tx = await _uow.BeginTransactionAsync();
            var order = await _uow.OrdersRepository.GetWithItemsAndProductsAsync(orderId);

            if (order is null) return 0;
            if (order.Status != OrderStatus.Pending) return 0;
            if (DateTime.UtcNow > order.ReservationExpiresAt) return 0;

            foreach (var it in order.Items)
            {
                var p = it.Product!;
                p.ReservedQty -= it.Quantity;
                p.Qty -= it.Quantity;
                if (p.ReservedQty < 0) p.ReservedQty = 0;
                _uow.Products.Update(p);
            }

            order.Status = OrderStatus.Paid;
            order.PaidAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync();
            await tx.CommitAsync();
            return 1;
        }

        public async Task<int> CancelOrExpireOrderAsync(int orderId)
        {
            await using var tx = await _uow.BeginTransactionAsync();
            var order = await _uow.OrdersRepository.GetWithItemsAndProductsAsync(orderId);

            if (order is null) return 0;
            if (order.Status != OrderStatus.Pending) return 0;

            foreach (var it in order.Items)
            {
                var p = it.Product!;
                p.ReservedQty -= it.Quantity;
                if (p.ReservedQty < 0) p.ReservedQty = 0;
                _uow.Products.Update(p);
            }

            order.Status = DateTime.UtcNow > order.ReservationExpiresAt
                ? OrderStatus.Expired
                : OrderStatus.Cancelled;

            await _uow.SaveChangesAsync();
            await tx.CommitAsync();
            return 1;
        }
    }
}
