using AskMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AskMe.Controllers
{
    public class AdminController : Controller
    {

        AskMeEntities db = new AskMeEntities();

        // GET: Admin
        public ActionResult ShowAdmins()
        {
            if (Session["Usernamess"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            string sql = "Select * from Admin";
            List<Admin> list = db.Admins.SqlQuery(sql).ToList();

            return View(list);
        }

        [HttpGet]
        public ActionResult AddAdmin(Admin obj)
        {
            if (Session["Usernamess"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            if (obj == null)
            {
                return View(obj);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddAdminPost(Admin model)
        {
            if (Session["Usernamess"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            if (db.Admins.Any(x => x.UserName == model.UserName) && model.AdminId == 0)
            {
                ViewBag.DuplicateMessage = "Username already exists";
                return View("AddAdmin");
            }
            //if(ModelState.IsValid)
            //{
                Admin obj = new Admin();
                obj.AdminId = model.AdminId;
                obj.Name = model.Name;
                obj.UserName = model.UserName;
                obj.AdminPassword = model.AdminPassword;

                if(model.AdminId == 0)
                {
                    db.Admins.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            //}

            ModelState.Clear();

            return View("AddAdmin");
        }

        public ActionResult DeleteAdmin(int AdminId)
        {
            if (Session["Usernamess"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            if (true)
            {
                ViewBag.notification = "You can not remove Yourself";
            }
            var res = db.Admins.Where(x => x.AdminId == AdminId).ToList().First();
            db.Admins.Remove(res);
            db.SaveChanges();

            var list = db.Admins.ToList();

            return View("ShowAdmins", list);
        }
        public ActionResult Logout()
        {

            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin model)
        {
            var checklogin = db.Admins.Where(x => x.UserName.Equals(model.UserName) && x.AdminPassword.Equals(model.AdminPassword)).FirstOrDefault();
            if (checklogin != null)
            {
                Session["IDss"] = model.AdminId.ToString();
                Session["Usernamess"] = model.UserName.ToString();
                return RedirectToAction("ShowAdmins", "Admin");
            }
            else
            {
                ViewBag.notification = "Wrong Infromation";
            }
            return View();
        }
    }
}