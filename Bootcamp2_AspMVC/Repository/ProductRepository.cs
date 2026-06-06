using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;

namespace Bootcamp2_AspMVC.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) => _db = db;

        public void Update(Product product) => _db.Products.Update(product);

        public void UpdateRange(IEnumerable<Product> products) => _db.Products.UpdateRange(products);
    }
}
