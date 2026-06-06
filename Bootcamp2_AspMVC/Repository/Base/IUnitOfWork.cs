using Bootcamp2_AspMVC.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bootcamp2_AspMVC.Repository.Base
{
    public interface IUnitOfWork
    {
        IRepoProduct Products { get; }

        IRepoEmployee Employees { get; }
        IRepoCategory RepoCategory { get; }

        IRepository<Category> Categories { get; }
        IRepository<Permission> Permissions { get; }

        // IRepository<Employee> Employees { get; }
        ICartItemRepository CartItemsRepository { get; }
        IProductRepository ProductsRepository { get; }
        IOrderRepository OrdersRepository { get; }

        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();


        void Save();




    }
}
