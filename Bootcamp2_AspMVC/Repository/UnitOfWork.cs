using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bootcamp2_AspMVC.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new RepoProduct(_context);
            Categories = new MainRepository<Category>(_context);
            Employees = new  RepoEmployee(_context);
            Permissions= new MainRepository<Permission>(_context);
            RepoCategory = new RepoCategory(_context);
            CartItemsRepository = new CartItemRepository(_context);
            ProductsRepository = new ProductRepository(_context);
            OrdersRepository = new OrderRepository(_context);

        }

        public IRepoProduct Products { get; }

        public IRepository<Category> Categories { get; }
        public IRepository<Permission> Permissions { get; }

        //public IRepository<Employee> Employees { get; }
        public IRepoEmployee Employees { get; }

        public IRepoCategory RepoCategory { get;}
        public ICartItemRepository CartItemsRepository { get; }
        public IProductRepository ProductsRepository { get; }
        public IOrderRepository OrdersRepository { get; }


        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync() =>
            _context.Database.BeginTransactionAsync();

        public void Save()
        {
            _context.SaveChanges();
        }



    }
}
