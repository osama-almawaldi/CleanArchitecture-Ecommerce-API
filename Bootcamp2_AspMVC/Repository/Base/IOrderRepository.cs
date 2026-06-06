using Bootcamp2_AspMVC.Models;

namespace Bootcamp2_AspMVC.Repository.Base
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetWithItemsAndProductsAsync(int orderId);
    }
}
