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
    public class UserProgressesController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // GET: Admin/UserProgresses
        public ActionResult Index()
        {
            var userProgresses = db.UserProgresses.Include(u => u.Lesson).Include(u => u.User);
            return View(userProgresses.ToList());
        }

        // GET: Admin/UserProgresses/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserProgress userProgress = db.UserProgresses.Find(id);
        //    if (userProgress == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userProgress);
        //}

        // GET: Admin/UserProgresses/Create
        public ActionResult Create()
        {
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Admin/UserProgresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProgressID,UserID,LessonID,Score,Completed")] UserProgress userProgress)
        {
            if (ModelState.IsValid)
            {
                db.UserProgresses.Add(userProgress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", userProgress.LessonID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", userProgress.UserID);
            return View(userProgress);
        }

        // GET: Admin/UserProgresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProgress userProgress = db.UserProgresses.Find(id);
            if (userProgress == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", userProgress.LessonID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", userProgress.UserID);
            return View(userProgress);
        }

        // POST: Admin/UserProgresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgressID,UserID,LessonID,Score,Completed")] UserProgress userProgress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProgress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", userProgress.LessonID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", userProgress.UserID);
            return View(userProgress);
        }

        // GET: Admin/UserProgresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProgress userProgress = db.UserProgresses.Find(id);
            if (userProgress == null)
            {
                return HttpNotFound();
            }
            return View(userProgress);
        }

        // POST: Admin/UserProgresses/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int ProgressID)
        {
            try
            {
                if (ProgressID <= 0)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var progressID = db.UserProgresses.Find(ProgressID);
                if (progressID == null)
                {
                    return Json(new { success = false, message = "Bài học không tồn tại!" });
                }

                db.UserProgresses.Remove(progressID);
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
