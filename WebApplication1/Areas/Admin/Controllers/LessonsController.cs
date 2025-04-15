using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class LessonsController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // GET: Admin/Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Topic);
            return View(lessons.ToList());
        }

        // GET: Admin/Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessons lessons = db.Lessons.Find(id);
            if (lessons == null)
            {
                return HttpNotFound();
            }
            return View(lessons);
        }

        // GET: Admin/Lessons/Create
        public ActionResult Create()
        {
            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicName");
            return View();
        }

        // POST: Admin/Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonID,LessonName,LessonContent,LessonType,DifficultyLevel,Score,TopicID")] Lessons lessons)
        {
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lessons);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicName", lessons.TopicID);
            return View(lessons);
        }

        // GET: Admin/Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessons lessons = db.Lessons.Find(id);
            if (lessons == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicName", lessons.TopicID);
            return View(lessons);
        }

        // POST: Admin/Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonID,LessonName,LessonContent,LessonType,DifficultyLevel,Score,TopicID")] Lessons lessons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lessons).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicName", lessons.TopicID);
            return View(lessons);
        }

        // GET: Admin/Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessons lessons = db.Lessons.Find(id);
            if (lessons == null)
            {
                return HttpNotFound();
            }
            return View(lessons);
        }

        // POST: Admin/Lessons/Delete/5

        [HttpPost]

        public ActionResult DeleteConfirmed(int LessonID)
        {
            try
            {
                if (LessonID <= 0)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var lessonID = db.Lessons.Find(LessonID);
                if (lessonID == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại!" });
                }

                db.Lessons.Remove(lessonID);
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
