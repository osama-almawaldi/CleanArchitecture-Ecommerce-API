using Bootcamp2_AspMVC.Models;

namespace Bootcamp2_AspMVC.Repository.Base
{
    public interface ICartItemRepository
    {
        Task<List<CartItem>> GetByCustomerWithProductAsync(int customerId);
        void RemoveRange(IEnumerable<CartItem> items);
    }
}
