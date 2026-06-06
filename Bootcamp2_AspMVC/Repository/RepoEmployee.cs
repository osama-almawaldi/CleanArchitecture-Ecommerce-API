using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;

namespace Bootcamp2_AspMVC.Repository
{
    public class RepoEmployee : MainRepository<Employee>, IRepoEmployee
    {
        private readonly ApplicationDbContext _context;
        public RepoEmployee(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Employee Login(string username, string password)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.Username == username && e.Password == password );
            return emp;
        }


        public IEnumerable<Employee> FindAllEmployee()
        {
            return _context.Employees.ToList().Where(e=>!e.IsDelete);
        }




    }

}
