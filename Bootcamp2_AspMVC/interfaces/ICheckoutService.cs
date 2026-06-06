namespace Bootcamp2_AspMVC.interfaces
{
    public interface ICheckoutService
    {
        Task<(bool ok, string message, int? orderId)> CreateOrderFromCartAsync(int customerId, TimeSpan? reservationDuration = null);
        Task<int> CancelOrExpireOrderAsync(int orderId); // يعيد المخزون المحجوز
        Task<int> MarkOrderPaidAsync(int orderId);       // يُثبت الحجز ويُخصم من Stock
    }
}
