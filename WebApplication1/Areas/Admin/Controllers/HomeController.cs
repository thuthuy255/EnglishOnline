using Model.DTO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Users;

namespace WebApplication1.Areas.Admin.Controllers
{

    
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class HomeController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();
        // GET: Admin/HomeƯ
        public ActionResult Index()
        {
            var listUser = db.Users.ToList().Count();
            var listTopic = db.Topic.ToList().Count();
            var listLessonComplete = db.UserProgresses.Where(p => p.Completed == true).ToList().Count();
            var listQuestion = db.Questions.ToList().Count();
            var listUserCustomer = db.Users.OrderByDescending(p => p.CreatedAt).Take(7).Where(p=>p.Role =="User").ToList();
            var listLessonHistory1 = (from lh in db.UserProgresses
                                     join l in db.Lessons on lh.LessonID equals l.LessonID
                                     join u in db.Users on lh.UserID equals u.UserID
                                     select new ListUserResponse
                                     {
                                         LessonTitle = l.LessonName,
                                         UserName = u.Username,
                                         Score = lh.Score
                                     })
                         .Take(5)
                         .ToList();

            ViewBag.listTopic = listTopic;
            ViewBag.listUser = listUser;
            ViewBag.listLessonComplete = listLessonComplete;
            ViewBag.listLessonHistory = listLessonHistory1;
            ViewBag.listQuestion = listQuestion;
            ViewBag.listUserCustomer = listUserCustomer;
            return View(listLessonHistory1);
        }
    }
}