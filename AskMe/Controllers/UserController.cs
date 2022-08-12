using AskMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AskMe.Controllers
{
    public class UserController : Controller
    {
        AskMeEntities db = new AskMeEntities();

        // GET: User
        [HttpGet]
        public ActionResult UserRegistration()
        {
            User userModel = new User();

            return View(userModel);
        }

        [HttpPost]
        public ActionResult UserRegistration(User userModel)
        {
            using (AskMeEntities dbModel = new AskMeEntities())
            {
                if(dbModel.Users.Any(x => (x.UserName == userModel.UserName
                                        || x.Email == userModel.Email)))
                {
                    ViewBag.DuplicateMessage = "Username or Email already exists";
                    return View("UserRegistration", new User());
                }

                dbModel.Users.Add(userModel);
                dbModel.SaveChanges();
            }

            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successfull!";

            return View("UserRegistration", new User());
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult UserLogin()
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            User userModel = new User();

            return View(userModel);
        }

        [HttpPost]
        public ActionResult UserLogin(User log)
        {
            var count = db.Users.Where(x => x.Email == log.Email 
                                            && x.UserPassword == log.UserPassword).Count();

            if(count > 0)
            {
                string sql = @"select * from Users where Email = '" + log.Email.ToString() + "'";

                var list = db.Users.SqlQuery(sql).ToList();

                Session["username"] = list[0].UserName;
                Session["userId"] = list[0].UserId;
                Session["successfulLogIn"] = 1;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Email or Password did not match.";
                return View();
            }
        }

        public ActionResult UserLogout()
        {
            Session.Remove("username");
            Session.Remove("userId");

            return RedirectToAction("Index", "Home");
        }
    }
}