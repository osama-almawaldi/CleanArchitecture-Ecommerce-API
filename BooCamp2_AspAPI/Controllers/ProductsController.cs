using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BootCamp2_AspAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]

        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            var listData = products.Select(p => new ProductMobileDto
            {
                Id = p.Id,
                uid = p.uid,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Qty = p.Qty,
                ReservedQty = p.ReservedQty,
                AvailableQty = p.Qty - p.ReservedQty,
                Description = p.Description,
                ImageUrl = "https://1hww33bb-7036.uks1.devtunnels.ms"+ p.ImageUrl

            }).ToList();
            return Ok(listData);
        }


        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var p = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            var dto = new ProductMobileDto 
            {
                Id = p.Id,
                uid = p.uid,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                Qty = p.Qty,
                ReservedQty = p.ReservedQty,
                AvailableQty = p.Qty - p.ReservedQty,
                Description = p.Description,
                ImageUrl = "https://1hww33bb-7036.uks1.devtunnels.ms" + p.ImageUrl



            };

            return Ok(dto);
        }
    

    
    
    
    }
}
