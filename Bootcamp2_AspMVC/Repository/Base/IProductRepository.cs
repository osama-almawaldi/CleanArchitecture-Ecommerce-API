using Bootcamp2_AspMVC.Models;

namespace Bootcamp2_AspMVC.Repository.Base
{
    public interface IProductRepository
    {
        void Update(Product product);
        void UpdateRange(IEnumerable<Product> products);
    }
}
