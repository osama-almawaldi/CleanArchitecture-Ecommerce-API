using Bootcamp2_AspMVC.Models;

namespace Bootcamp2_AspMVC.Repository.Base
{
    public interface IRepoEmployee : IRepository<Employee>
    {

        Employee Login(string username, string password);

        IEnumerable<Employee> FindAllEmployee();
    }
}
