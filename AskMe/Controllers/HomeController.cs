using AskMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AskMe.Controllers
{
    public class HomeController : Controller
    {
        AskMeEntities db = new AskMeEntities();

        public ActionResult Index(int? page)
        {
            List<Question> qList = db.Questions.Where(x => x.QStatus == 1).ToList();

            if(page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }

            int limit = 5;
            int start = (int)(page - 1) * limit;
            int total = qList.Count;
            int totalPages = (int)Math.Ceiling((double)total / (double)limit);

            ViewBag.totalQuestions = total;
            ViewBag.totalPages = totalPages;
            ViewBag.currentPage = page;
            ViewBag.qList = qList.Skip(start).Take(limit);

            return View();
        }

        public ActionResult ShowQuestion(Question qs)
        {
            User qUser = db.Users.Where(x => x.UserId == qs.UserId).ToList().First();
            List<Answer> ans = db.Answers.Where(x => x.QuestionId == qs.QuestionId).ToList();

            ViewBag.qs = qs;
            ViewBag.qUser = qUser;
            ViewBag.ans = ans;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Ask()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            List<Category> categoryList = db.Categories.ToList();
            ViewBag.categoryList = categoryList;

            return View();
        }

        [HttpPost]
        public ActionResult Ask(Question log)
        {
            log.UserId = Convert.ToInt32(Session["userId"]);
            log.CreationDate = Convert.ToString(DateTime.Now);
            if(log.QStatus == null)
            {
                log.QStatus = 0;
            }
            log.Solved = 0;
            log.VoteCount = 0;

            db.Questions.Add(log);
            db.SaveChanges();

            //return View("Index");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MyQuestions()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            int userId = Convert.ToInt32(Session["userId"]);

            List<Question> myQList = db.Questions.Where(x => x.UserId == userId ).ToList();
            ViewBag.myQList = myQList;

            return View();
        }
    }
}