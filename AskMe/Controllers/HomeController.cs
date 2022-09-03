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
            //User qUser = db.Users.Where(x => x.UserId == qs.UserId).ToList().First();
            //List<Answer> ans = db.Answers.Where(x => x.QuestionId == qs.QuestionId).ToList();

            //ViewBag.qs = qs;
            //ViewBag.qUser = qUser;
            //ViewBag.ans = ans;
            ViewBag.qid = qs.QuestionId;

            return View();
        }

        public ActionResult GenerateVote(int qid, int? aid, int val)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            Question qs = db.Questions.Where(x => x.QuestionId == qid).ToList().First();

            int uid = Convert.ToInt32(Session["userId"]);
            //int id = uid;

            if (qid > 0 && aid == 0)
            {
                var count = db.Votes.Where(x => x.U_Id == uid
                                            && x.Q_Id == qid).Count();

                if (count > 0)
                {
                    //Session["voteTwice"] = 1;
                    //return RedirectToAction("ShowQuestion", qs);

                    Vote vote = db.Votes.Where(x => x.U_Id == uid && x.Q_Id == qid).ToList().First();

                    if (vote.Upvote == 1)
                    {
                        if (val == 1)
                        {
                            db.Votes.Remove(vote);
                            db.SaveChanges();
                        }
                        else
                        {
                            vote.Upvote = -1;
                            db.Entry(vote).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        if (val == -1)
                        {
                            db.Votes.Remove(vote);
                            db.SaveChanges();
                        }
                        else
                        {
                            vote.Upvote = 1;
                            db.Entry(vote).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("ShowQuestion", qs);
                }

                if(val != 0)
                {
                    Vote vote = new Vote();
                    vote.Upvote = vote.Downvote = val;
                    vote.Q_Id = qid;
                    vote.U_Id = uid;

                    db.Votes.Add(vote);
                    db.SaveChanges();

                    return RedirectToAction("ShowQuestion", qs);
                }
            }
            else if(aid > 0)
            {
                var count = db.Votes.Where(x => x.U_Id == uid
                                            && x.A_Id == aid).Count();

                if (count > 0)
                {
                    //Session["voteTwice"] = 1;
                    //return RedirectToAction("ShowQuestion", qs);

                    Vote vote = db.Votes.Where(x => x.U_Id == uid && x.A_Id == aid).ToList().First();

                    if(vote.Upvote == 1)
                    {
                        if(val == 1)
                        {
                            db.Votes.Remove(vote);
                            db.SaveChanges();
                        }
                        else
                        {
                            vote.Upvote = -1;
                            db.Entry(vote).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        if (val == -1)
                        {
                            db.Votes.Remove(vote);
                            db.SaveChanges();
                        }
                        else
                        {
                            vote.Upvote = 1;
                            db.Entry(vote).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("ShowQuestion", qs);
                }

                if (val != 0)
                {
                    Vote vote = new Vote();
                    vote.Upvote = vote.Downvote = val;
                    vote.A_Id = aid;
                    vote.U_Id = uid;

                    db.Votes.Add(vote);
                    db.SaveChanges();

                    return RedirectToAction("ShowQuestion", qs);
                }
            }

            return RedirectToAction("ShowQuestion", qs);
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

            return View(myQList);
        }
        [HttpGet]
        public ActionResult EditQuestion(Question obj)
        {

            List<Category> categoryList = db.Categories.ToList();
            ViewBag.categoryList = categoryList;
         

            return View(obj);
        }
        [HttpPost]
        public ActionResult EditQuestionPost(Question obj)
        {

            if (obj.QStatus == null) obj.QStatus = 0;
            else obj.QStatus = 1;
            if (obj.Solved == null) obj.Solved = 0;
            else obj.Solved = 1;
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();




            return RedirectToAction("MyQuestions");
        }

        public ActionResult Delete(int QuestionId)
        {
            if (false)
            {
                var answertodlt = db.Answers.Where(x => x.QuestionId == QuestionId).First();
                db.Answers.Remove(answertodlt);
                db.SaveChanges();
            }

            var res = db.Questions. Where(x => x.QuestionId == QuestionId).First();
            db.Questions.Remove(res);
            db.SaveChanges();
            var list = db.Categories.ToList();
            return RedirectToAction("Index");
        }

        public ActionResult AnswerQuestion(Answer qs)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            if (qs.Content == null)
            {
                ViewBag.notification = "Please Answer Correctly";
                Question qq1 = db.Questions.Where(x => x.QuestionId == qs.QuestionId).ToList().First();
                return RedirectToAction("ShowQuestion", qq1);
            }
            Answer obj = new Answer();


            obj.QuestionId = qs.QuestionId;
            obj.UserId = qs.UserId;
            obj.Content = qs.Content;
            obj.voteCount = 0;
            obj.CreationDate = Convert.ToString(DateTime.Now);

            Question qq = db.Questions.Where(x => x.QuestionId == qs.QuestionId).ToList().First();

            db.Answers.Add(obj);
            db.SaveChanges();
            ViewBag.notification = "Added";

            return RedirectToAction("ShowQuestion", qq);

        }
    }
}