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
                //string sql = @"select * from Users where Email = '" + log.Email + "'";

                //var list = db.Users.SqlQuery(sql).ToList();

                User user = db.Users.Where(x => x.Email == log.Email
                                            && x.UserPassword == log.UserPassword).ToList().First();

                Session["username"] = user.UserName.ToString();
                Session["userId"] = user.UserId.ToString();
                Session["successfulLogIn"] = 1;
                //CurrentUser.Id = user.UserId;
                //string username = Session["username"].ToString();
                //string us2 = username;
                //int id = Convert.ToInt32(Session["userId"]);
                //int id2 = id;

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