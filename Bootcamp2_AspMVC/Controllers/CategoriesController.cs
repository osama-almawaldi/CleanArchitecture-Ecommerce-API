using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Dtos;
using Bootcamp2_AspMVC.Filters;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp2_AspMVC.Controllers
{
    [SessionAuthorize]
    public class CategoriesController : Controller
    {

       private readonly ApplicationDbContext _context;
        private readonly IRepository<Category> _repository;
        public CategoriesController( IRepository<Category> repository)
        {
            
            _repository = repository;

        }

   
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Category> category =_repository.FindAll();


            if (category.Any())
            {
                TempData["Sucees"] = "تم جلب البيانات بنجاح";
            }
            else
            {
                TempData["Error"] = "لا توجد بيانات لعرضها";

            }

            return View(category);

        }



        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = _context.Categories
                .Include(c => c.Products).Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Count = c.Products.Count()
                })
                .ToList();

            return Ok(categories); // يرجّع JSON
        }



        [HttpGet]
        public IActionResult Create()
        {
          
            return View();
        }


        //[HttpPost]
        //public IActionResult Create(Category category)
        //{



        //    try
        //    {
        //        if (category.Name == "100")
        //        {
        //            ModelState.AddModelError("CustomError", "Name Van not be Equal 100");
        //        }


        //        if (ModelState.IsValid)
        //        {
        //            category.Name = null;
        //            _context.Categories.Add(category);
        //            _context.SaveChanges();
        //            TempData["Add"] = "تم اضافة البيانات بنجاح";
        //            return RedirectToAction("Index");

        //        }
        //        else
        //        {
        //            return View(category);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "حدث خطأ أثناء إضافة البيانات: " + ex.Message);
        //        return View(category);
        //    }


        //}




        [HttpPost]
        public IActionResult Create(Category category)
        {



            try
            {
                if (category.Name == "100")
                {
                    ModelState.AddModelError("CustomError", "Name Van not be Equal 100");
                }


                if (ModelState.IsValid)
                {
                   _repository.Add(category);
                    TempData["Add"] = "تم اضافة البيانات بنجاح";
                    return RedirectToAction("Index");

                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء إضافة البيانات: " + ex.Message);
                return View(category);
            }


        }



        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var cate = _repository.FindById(Id);

            return View(cate);
        }


        [HttpPost]
        public IActionResult Edit(Category category)
        {
            category.uid = Guid.NewGuid().ToString();
            _repository.Update(category);
            TempData["Update"] = "تم تحديث البيانات بنجاح";
            return RedirectToAction("Index");


        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var cate = _repository.FindById(Id);


            return View(cate);
        }


        [HttpPost]
        public IActionResult Delete(Category category)
        {
        
            //_context.Categories.Remove(category);
            //_context.SaveChanges();

            _repository.Delete(category);
            TempData["Remove"] = "تم حذف البيانات بنجاح";
            return RedirectToAction("Index");


        }




    }
}
