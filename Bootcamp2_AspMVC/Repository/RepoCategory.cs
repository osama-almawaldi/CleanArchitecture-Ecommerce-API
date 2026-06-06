    using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;

namespace Bootcamp2_AspMVC.Repository
{
    public class RepoCategory : MainRepository<Category>, IRepoCategory
    {
        private readonly ApplicationDbContext _context;
        public RepoCategory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Category FindByUIdCategory(string uid)
        {
            Category Category= _context.Categories.FirstOrDefault(c => c.uid == uid);
            return Category;

        }

    }
}
