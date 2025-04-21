using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class AnswersController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();
        private CloudinaryService _cloudinaryService = new CloudinaryService();

        // GET: Admin/Answers
        public ActionResult Index()
        {
            var answers = db.Answers.Include(a => a.Question);
            return View(answers.ToList());
        }

        // GET: Admin/Answers/Create
        public ActionResult Create()
        {
            var lessons = db.Lessons.ToList(); // Đã sửa lỗi ở đây
            ViewBag.listlesson = lessons;
            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText");
            return View();
        }

        [HttpGet]
        public JsonResult GetQuestionByLessonID(int LessonID)
        {
            var questions = db.Questions
                              .Where(q => q.LessonID == LessonID)
                              .Select(q => new {
                                  QuestionID = q.QuestionID,
                                  QuestionText = q.QuestionText
                              }).ToList();

            return Json(questions, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AnswerID,AnswerText,IsCorrect,QuestionID")] Answers answers, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    try
                    {
                        var cloudService = new CloudinaryService();
                        var imageUrl = await cloudService.UploadImageAsync(ImageUpload);

                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            answers.ImagePath = imageUrl;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tải ảnh lên thất bại!");
                            return View(answers);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi tải ảnh lên: " + ex.Message);
                        return View(answers);
                    }
                }

                db.Answers.Add(answers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText", answers.QuestionID);
            return View(answers);
        }

        // GET: Admin/Answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Answers answers = db.Answers.Find(id);
            if (answers == null)
            {
                return HttpNotFound();
            }

            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText", answers.QuestionID);
            return View(answers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Answers answers, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    try
                    {
                        var cloudService = new CloudinaryService();
                        var imageUrl = await cloudService.UploadImageAsync(ImageUpload);

                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            answers.ImagePath = imageUrl;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tải ảnh lên thất bại!");
                            return View(answers);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi tải ảnh lên: " + ex.Message);
                        return View(answers);
                    }
                }

                db.Entry(answers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText", answers.QuestionID);
            return View(answers);
        }

        // GET: Admin/Answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Answers answers = db.Answers.Find(id);
            if (answers == null)
            {
                return HttpNotFound();
            }

            return View(answers);
        }

        // POST: Admin/Answers/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int AnswersID)
        {
            try
            {
                if (AnswersID <= 0)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var answers = db.Answers.Find(AnswersID);
                if (answers == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại!" });
                }

                db.Answers.Remove(answers);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Xóa thành công!";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa: " + ex.Message });
            }
        }

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
