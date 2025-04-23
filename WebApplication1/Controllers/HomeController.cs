using Model.EF;
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
            // Kiểm tra đăng nhập
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = (int)Session["UserID"];

            // Lấy danh sách tất cả topic và lessons
            var allTopics = db.Topic
                            .Include(t => t.Lessons)
                            .OrderBy(t => t.TopicID)
                            .ToList();

            // Lấy danh sách bài học đã hoàn thành của user
            var completedLessons = db.UserProgresses
                                  .Where(up => up.UserID == userId && up.Completed)
                                  .Select(up => up.LessonID)
                                  .ToList();

            // Xác định topic nào đã hoàn thành
            var topicStatus = new Dictionary<Guid, bool>();
            foreach (var topic in allTopics)
            {
                bool isCompleted = topic.Lessons.Any() &&
                                 topic.Lessons.All(l => completedLessons.Contains(l.LessonID));
                topicStatus[topic.TopicID] = isCompleted;
            }

            // Xác định topic nào được mở khóa
            var unlockedTopicIds = new HashSet<Guid>();

            // 1. Topic không có điều kiện tiên quyết luôn mở khóa
            foreach (var topic in allTopics.Where(t => t.PrerequisiteTopicId == null))
            {
                unlockedTopicIds.Add(topic.TopicID);
            }

            // 2. Topic có điều kiện tiên quyết mở khóa nếu topic trước đã hoàn thành
            foreach (var topic in allTopics.Where(t => t.PrerequisiteTopicId != null))
            {
                if (topicStatus.ContainsKey(topic.PrerequisiteTopicId.Value) &&
                    topicStatus[topic.PrerequisiteTopicId.Value])
                {
                    unlockedTopicIds.Add(topic.TopicID);
                }
            }

            // Truyền dữ liệu sang View
            ViewBag.TopicStatus = topicStatus;
            ViewBag.UnlockedTopicIds = unlockedTopicIds;
            ViewBag.UserCompletedLessons = completedLessons;
            ViewBag.AllTopics = allTopics; // Truyền tất cả topics

            return View(allTopics); // Trả về tất cả topics
        }
        public ActionResult Profile()
        {
            return View();
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
        public ActionResult InforUser()
        {
            return View();
        }
    }
}