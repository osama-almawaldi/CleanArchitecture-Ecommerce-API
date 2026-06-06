using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp2_AspMVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;


        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Logout()
        {
          
            HttpContext.Session.Clear();
            return View("Login");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var emp = _unitOfWork.Employees.Login(username, password);



            if (emp != null)
            {
                if (emp.Islock)
                {

                    ViewBag.Error = "تم اغلاق  هذا المستخدم  يرجي مراجعة الادمن";
                    return View();

                }

                if (emp.UserRoleId == 1)
                {

                    HttpContext.Session.SetString("Username", emp.FirstName);
                    HttpContext.Session.SetInt32("Id", emp.Id);
                    return RedirectToAction("Index", "Home");

                }
                else {


                    HttpContext.Session.SetString("UsernameEmployee", emp.FirstName);
                    HttpContext.Session.SetInt32("Id", emp.Id);
                    return RedirectToAction("Index", "HomeEmployee");


                }




          
            }

            //if (username == "admin" && password == "123")
            //{

            //    HttpContext.Session.SetString("Username", username);
            //    HttpContext.Session.SetInt32("Id", 100);
            //    return RedirectToAction("Index", "Home");
            //}

            ViewBag.Error = "خطا في اسم المستخدو او كلمة المرور";
            return View();

        }
    }
}
