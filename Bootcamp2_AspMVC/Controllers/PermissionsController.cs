

using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Permission = Bootcamp2_AspMVC.Models.Permission;

namespace Bootcamp2_AspMVC.Controllers
{
    public class PermissionsController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public PermissionsController(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork; 
        }




        private void CreateEmployeeSelectList()
        {
            IEnumerable<Employee> emps = _unitOfWork.Employees.FindAll();

            SelectList selectListItems = new SelectList(emps, "Id", "FirstName", 0);
            ViewBag.Employees = selectListItems;

        }






        public IActionResult Index()
        {
        var permissions=    _unitOfWork.Permissions.FindAll();
            return View(permissions);
        }


        [HttpGet]
        public IActionResult Create()
        {
            CreateEmployeeSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Permissions.Add(permission);
                _unitOfWork.Save();
                TempData["Sucees"] = "Permission Created Suceesfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Permission permission)
        {
            return View();
        }

    }
}
