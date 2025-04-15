using Model.EF;
using System.Collections.Generic;
using System.Data.Entity; // Thêm namespace này để dùng Include với lambda
using System.Linq;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class LessonController : Controller
    {
        private readonly EnglishOnlineDbContext db;

        public LessonController()
        {
            db = new EnglishOnlineDbContext();
        }

        // GET: Lesson/Lesson_3pic
        public ActionResult Lesson_3pic()
        {
            var questions = db.Questions
                .Include(q => q.Answers) // Sử dụng lambda cho type-safe
                .ToList();

            if (questions == null || !questions.Any())
            {
                // Xử lý trường hợp không có dữ liệu
                ViewBag.Message = "Hiện tại không có câu hỏi nào.";
                return View(new List<Questions>());
            }

            return View(questions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); // Giải phóng tài nguyên
            }
            base.Dispose(disposing);
        }
    }



}