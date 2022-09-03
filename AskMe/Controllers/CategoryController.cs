using AskMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AskMe.Controllers
{
    public class CategoryController : Controller
    {// GET: Category
        AskMeEntities dbobj = new AskMeEntities();
   
        public ActionResult AddCategory(Category model)
        {
            if(model.CategoryName== null) return View();
            Category obj = new Category();
            if (dbobj.Categories.Any(x => x.CategoryName == model.CategoryName))
            {
                ViewBag.notification = "This Category has already existed";
                return View();
            }
           
                obj.CategoryId = model.CategoryId;
                obj.CategoryName = model.CategoryName;

                if (model.CategoryId == 0)
                {
                    dbobj.Categories.Add(obj);
                    dbobj.SaveChanges();
                ViewBag.notification = "Category Added";
            }
                else
                {
                    dbobj.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    dbobj.SaveChanges();
                ViewBag.notification = "Category Name Updated";
            }

            ModelState.Clear();

           // var list = dbobj.Categories.ToList();
            return View();
        }

        public ActionResult CategoryList()
        {
            var res = dbobj.Categories.ToList();
            return View(res);
        }

        public ActionResult Delete(int CategoryId)
        {
           
            if (dbobj.Questions.Any(x => x.CategoryId == CategoryId))
            {
                ViewBag.notification = "This Category has Questions Cannot be deleted";
                var list = dbobj.Categories.ToList();
                return View("CategoryList", list);
            }
            else
            {
                var res = dbobj.Categories.Where(x => x.CategoryId == CategoryId).First();
                dbobj.Categories.Remove(res);
                dbobj.SaveChanges();
                var list = dbobj.Categories.ToList();
                return View("CategoryList", list);
            }

        }
    }
}