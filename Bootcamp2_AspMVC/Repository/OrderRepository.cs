using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(Order order) => await _db.Orders.AddAsync(order);

        public Task<Order?> GetWithItemsAndProductsAsync(int orderId) =>
            _db.Orders
               .Include(o => o.Items)
               .ThenInclude(i => i.Product)
               .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}
