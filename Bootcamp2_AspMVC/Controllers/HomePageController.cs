using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Filters;
using Bootcamp2_AspMVC.interfaces;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Controllers
{
    public class HomePageController : Controller
    {



        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly ICheckoutService _checkoutService;

        public HomePageController(IUnitOfWork unitOfWork, ApplicationDbContext context, ICheckoutService checkoutService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _checkoutService = checkoutService;
        }



        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Products.FindAllproducts();
            return View(products);
        }





        public IActionResult Details(int Id)
        {
           var products = _unitOfWork.Products.FindByIdproduct(Id);
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomerSessionAuthorize]
        public IActionResult AddToCart(int id,  int custId ,int qty = 1)
        {
            var p = _unitOfWork.Products.FindById(id);
            if (p == null) return NotFound();

            var item = new CartItem
            {
                ProductId = p.Id,
                Quantity = qty,
                CustomerId = custId

            };


            _context.CartItems.Add(item);
            _context.SaveChanges();


            return RedirectToAction("Cart");

        }




        [HttpPost]
        [CustomerSessionAuthorize]
        public async Task<IActionResult> CreateOrder(int custId)
        {
            int? sessionId = HttpContext.Session.GetInt32("IdCustomer");
            if (custId != sessionId)
                return Unauthorized();

            // استدعاء خدمة إنشاء الأوردر (اللي شرحناها قبل كده)
            var (ok, msg, orderId) = await _checkoutService.CreateOrderFromCartAsync(custId);

            if (!ok)
            {
                TempData["Error"] = msg;
                return RedirectToAction("Cart", new { custId });
            }

            TempData["Success"] = "تم إنشاء الطلب بنجاح.";
            return RedirectToAction("Details", "Orders", new { id = orderId });
        }




        [CustomerSessionAuthorize]
        public IActionResult Cart(int custId)
        {
            
            if(custId!=0)
            {
                int? Id = HttpContext.Session.GetInt32("IdCustomer");
                if (custId != Id)
                {
                    return Unauthorized();

                }
            }
            var cartItems = _context.CartItems
                .Include(c => c.Product).Where(e => e.CustomerId == custId)
                .ToList();

            return View(cartItems); // بيرجع View اسمه Cart.cshtml
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int qty)
        {
            var item = _context.CartItems.Find(id);
            if (item != null && qty > 0)
            {
                item.Quantity = qty;
               // _context.CartItems.Update(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Cart");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var item = _context.CartItems.Find(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Cart");
        }

    }

}

