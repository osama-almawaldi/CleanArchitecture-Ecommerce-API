using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Repository
{
    public class RepoProduct : MainRepository<Product>, IRepoProduct
    {
        private readonly ApplicationDbContext _context;
        public RepoProduct(ApplicationDbContext context):base(context)
        {
            _context = context;
        }


        public  IEnumerable<Product> FindAllproducts()
        {
            IEnumerable<Product> products = _context.Products.Include(e => e.Category).AsNoTracking().ToList();
            return products;
        }

        public Product FindByIdproduct(int id)
        {
            Product product = _context.Products.Include(e => e.Category).AsNoTracking().FirstOrDefault(p => p.Id == id);
            return product;
        }

        public Product FindByUIdproduct(string uid)
        {
            Product product = _context.Products.FirstOrDefault(c => c.uid == uid);
            return product;

        }


    }
}
