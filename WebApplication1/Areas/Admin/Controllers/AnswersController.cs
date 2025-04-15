using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class AnswersController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // GET: Admin/Answers
        public ActionResult Index()
        {
            var answers = db.Answers.Include(a => a.Question);
            return View(answers.ToList());
        }

        // GET: Admin/Answers/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Answers answers = db.Answers.Find(id);
        //    if (answers == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(answers);
        //}

        // GET: Admin/Answers/Create
        public ActionResult Create()
        {
            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText");
            return View();
        }

        // POST: Admin/Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnswerID,AnswerText,IsCorrect,QuestionID")] Answers answers, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                // Nếu có file ảnh được upload
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    // Tạo tên file duy nhất
                    var fileName = Path.GetFileName(ImageUpload.FileName);
                    var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                    // Đường dẫn vật lý lưu ảnh
                    var path = Path.Combine(Server.MapPath("~/Uploads/Images"), newFileName);

                    // Tạo thư mục nếu chưa có
                    Directory.CreateDirectory(Server.MapPath("~/Uploads/Images"));

                    // Lưu file
                    ImageUpload.SaveAs(path);

                    // Lưu đường dẫn URL ảnh (để FE dễ lấy)
                    var baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                    answers.ImagePath = baseUrl + "/Uploads/Images/" + newFileName;
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

        // POST: Admin/Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Answers answers, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImageUpload.FileName);
                    var folderPath = Server.MapPath("~/Uploads/Answers");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var fullPath = Path.Combine(folderPath, fileName);
                    ImageUpload.SaveAs(fullPath);
                    answers.ImagePath = "~/Uploads/Answers/" + fileName;
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
