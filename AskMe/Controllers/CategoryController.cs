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