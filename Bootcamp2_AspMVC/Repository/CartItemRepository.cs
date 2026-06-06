using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _db;

        public CartItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<List<CartItem>> GetByCustomerWithProductAsync(int customerId)
        {
            return _db.CartItems
                      .Include(c => c.Product)
                      .Where(c => c.CustomerId == customerId)
                      .ToListAsync();
        }

        public void RemoveRange(IEnumerable<CartItem> items)
        {
            _db.CartItems.RemoveRange(items);
        }
    }
}
