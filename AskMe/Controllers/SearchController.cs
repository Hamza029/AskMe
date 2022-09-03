using AskMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AskMe.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        AskMeEntities dbobj = new AskMeEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(String s)
        {
            return View();
        }
        public ActionResult SearchList(Question obj)
        {
            var contents = from x in dbobj.Questions select x;
            if (!String.IsNullOrEmpty(obj.Content))
            {
                contents= contents.Where(x=>x.Content.Contains(obj.Content));
            }

            ViewBag.qList=contents.ToList();
            return View();
        }
        /**/
    }
}