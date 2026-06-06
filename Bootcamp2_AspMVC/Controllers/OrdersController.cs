using Bootcamp2_AspMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Details(int id)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }
    }

}
