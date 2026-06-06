using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.interfaces;
using Bootcamp2_AspMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Services
{
    public class CheckoutService22 : ICheckoutService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CheckoutService22> _logger;

        public CheckoutService22(ApplicationDbContext db, ILogger<CheckoutService22> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<(bool ok, string message, int? orderId)> CreateOrderFromCartAsync(int customerId, TimeSpan? reservationDuration = null)
        {
            reservationDuration ??= TimeSpan.FromMinutes(30);

            var cart = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.CustomerId == customerId)
                .ToListAsync();

            if (!cart.Any())
                return (false, "السلة فارغة.", null);

            // تحقق التوفر
            foreach (var ci in cart)
            {
                var available = ci.Product.Qty - ci.Product.ReservedQty;
                if (ci.Quantity <= 0)
                    return (false, $"الكمية غير صالحة للمنتج {ci.Product.ProductName}.", null);

                if (ci.Quantity > available)
                    return (false, $"لا توجد كمية كافية من المنتج {ci.Product.ProductName}. المتاح: {available}.", null);
            }

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // حجز الكميات
                foreach (var ci in cart)
                {
                    ci.Product.ReservedQty += ci.Quantity; // حجز مؤقت
                    _db.Products.Update(ci.Product);
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
                        UnitPrice = ci.Product.Price
                    };
                    subtotal += item.UnitPrice * item.Quantity;
                    order.Items.Add(item);
                }
                order.Subtotal = subtotal;

                _db.Orders.Add(order);

                // تفريغ السلة
                _db.CartItems.RemoveRange(cart);

                await _db.SaveChangesAsync();
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

        // عند الدفع: نثبت الحجز ونخصم فعلياً من المخزون
        public async Task<int> MarkOrderPaidAsync(int orderId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            var order = await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return 0;
            if (order.Status != OrderStatus.Pending) return 0;

            // لو انتهت صلاحية الحجز لا نكمل
            if (DateTime.UtcNow > order.ReservationExpiresAt)
                return 0;

            foreach (var it in order.Items)
            {
                // تثبيت: خصم من StockQty وإنقاص المحجوز
                it.Product.ReservedQty -= it.Quantity;
                it.Product.Qty -= it.Quantity;
                _db.Products.Update(it.Product);
            }

            order.Status = OrderStatus.Paid;
            order.PaidAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return 1;
        }

        // إلغاء/انتهاء صلاحية: إعادة المحجوز
        public async Task<int> CancelOrExpireOrderAsync(int orderId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            var order = await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return 0;
            if (order.Status != OrderStatus.Pending) return 0;

            foreach (var it in order.Items)
            {
                it.Product.ReservedQty -= it.Quantity;
                if (it.Product.ReservedQty < 0) it.Product.ReservedQty = 0; // حماية
                _db.Products.Update(it.Product);
            }

            order.Status = DateTime.UtcNow > order.ReservationExpiresAt
                ? OrderStatus.Expired
                : OrderStatus.Cancelled;

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return 1;
        }
    }
}
