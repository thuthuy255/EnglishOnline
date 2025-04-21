using Microsoft.AspNet.Identity;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class LessonController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        public LessonController()
        {
            db = new EnglishOnlineDbContext();
        }

        public ActionResult Lesson_3pic(int LessonID)
        {
            if(LessonID==null)
            {
                ViewBag.Message = "Không có bài học này";
                return View();
            }
            var questions = db.Questions
                .Include(q => q.Answers)
                .Where(q => q.LessonID == LessonID)
                .ToList();

            if (questions == null || !questions.Any())
            {
                // Xử lý trường hợp không có dữ liệu
                ViewBag.Message = "Hiện tại không có câu hỏi nào.";
                return View(new List<Questions>());
            }

            return View(questions);
        }
        [HttpPost]
        public ActionResult SaveUserScores(List<int> lessonIds)
        {
            try
            {
                // Lấy UserId từ session
                int? userId = Session["UserId"] as int?;

                if (userId == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để lưu điểm!" });
                }

                // Kiểm tra xem danh sách bài học có rỗng không
                if (lessonIds == null || lessonIds.Count == 0)
                {
                    return Json(new { success = false, message = "Danh sách bài học không hợp lệ." });
                }

                // Lấy điểm của các bài học và tính tổng điểm
                int totalScore = 0;
                var lessonsNotFound = new List<int>(); // Lưu các bài học không tìm thấy

                foreach (var lessonId in lessonIds)
                {
                    var lesson = db.Lessons.Find(lessonId);
                    if (lesson != null)
                    {
                        // Kiểm tra nếu đã có tiến độ học cho bài học này
                        var existingProgress = db.UserProgresses
                            .FirstOrDefault(up => up.UserID == userId.Value && up.LessonID == lessonId);

                        if (existingProgress != null)
                        {
                            // Cộng dồn điểm nếu đã có
                            existingProgress.Score += lesson.Score;
                            existingProgress.Completed = true; // Đánh dấu đã hoàn thành
                        }
                        else
                        {
                            // Nếu chưa có, tạo mới
                            var userProgress = new UserProgress
                            {
                                UserID = userId.Value,
                                LessonID = lessonId,
                                Score = lesson.Score,
                                Completed=true,
                            };
                            db.UserProgresses.Add(userProgress);
                        }

                        // Cộng điểm vào tổng điểm
                        totalScore += lesson.Score;
                    }
                    else
                    {
                        lessonsNotFound.Add(lessonId); // Ghi nhận bài học không tìm thấy
                    }
                }


                // Kiểm tra nếu có bài học không tìm thấy
                if (lessonsNotFound.Any())
                {
                    return Json(new { success = false, message = "Một số bài học không tồn tại: " + string.Join(", ", lessonsNotFound) });
                }

                // Lưu tất cả các thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Trả về thông báo thành công
                return Json(new { success = true, message = "Điểm đã được lưu thành công!", totalScore = totalScore });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu điểm: " + ex.Message });
            }
        }






        // Phương thức Dispose để giải phóng tài nguyên
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
