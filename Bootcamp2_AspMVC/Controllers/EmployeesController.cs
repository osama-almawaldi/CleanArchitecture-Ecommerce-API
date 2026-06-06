using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Filters;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp2_AspMVC.Controllers
{
    [SessionAuthorize]
    public class EmployeesController : Controller
    {
        //private readonly IRepository<Employee> _repository;

        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            var  employees = _unitOfWork.Employees.FindAllEmployee();
            if (employees.Any())
            {
                TempData["Sucees"] = "تم جلب البيانات بنجاح";
            }
            else
            {
                TempData["Error"] = "لا توجد بيانات لعرضها";
            }
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Employee employee)
        {

            //_context.Employees.Add(employee);
            //_context.SaveChanges();
            _unitOfWork.Employees.Add(employee);
            _unitOfWork.Save();
            TempData["Add"] = "تم اضافة البيانات بنجاح";
            return RedirectToAction("Index");


        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            //var cate = _context.Employees.Find(Id);
            var cate = _unitOfWork.Employees.FindById(Id);

            return View(cate);
        }


        [HttpPost]
        public IActionResult Edit(Employee emp)
        {

            //_context.Employees.Update(emp);
            //_context.SaveChanges();

            _unitOfWork.Employees.Update(emp);
            _unitOfWork.Save();
            TempData["Update"] = "تم تعديل البيانات بنجاح";
            return RedirectToAction("Index");


        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            //var cate = _context.Employees.Find(Id);
            var cate = _unitOfWork.Employees.FindById(Id);

            return View(cate);
        }


        [HttpPost]
        public IActionResult Deletepost(int Id)
        {

            //_context.Employees.Remove(emp);
            //_context.SaveChanges();
            //_unitOfWork.Employees.Delete(emp);
            //_unitOfWork.Save();

            var emp = _unitOfWork.Employees.FindById(Id);
            emp.IsDelete = true;
            emp.UserDelete = HttpContext.Session.GetInt32("Id") ?? 0;
            emp.DeleteDate = DateTime.Now;
            _unitOfWork.Employees.Update(emp);
            _unitOfWork.Save();
            TempData["Remove"] = "تم حذف البيانات بنجاح";
            return RedirectToAction("Index");


        }





    }
}
