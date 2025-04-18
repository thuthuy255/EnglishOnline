﻿using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using static Users;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        [Authorize(Roles = nameof(UserRole.User))]

        public ActionResult Index()
        {
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
        public ActionResult News()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Learn()
        {
            var topics = db.Topic
          .Include("Lessons")
          .OrderBy(t => t.PrerequisiteTopic == null ? 0 : 1) // Đưa topic không có điều kiện lên đầu
          .ThenBy(t => t.TopicName) // Sắp xếp phụ nếu cần
          .ToList();
            return View(topics);
        }
        public ActionResult Pronunciation()
        {
            return View();
        }
        public ActionResult DailyTasks()
        {
            return View();
        }
        public ActionResult SeeMore()
        {
            return View();
        }
    }
}