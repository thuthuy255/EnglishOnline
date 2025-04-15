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
    public class BadgesController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // GET: Admin/Badges
        public ActionResult Index()
        {
            return View(db.Badges.ToList());
        }

        // GET: Admin/Badges/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Badges badges = db.Badges.Find(id);
        //    if (badges == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(badges);
        //}

        // GET: Admin/Badges/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Badges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BadgeID,Name,Description")] Badges badges)
        {
            if (ModelState.IsValid)
            {
                db.Badges.Add(badges);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(badges);
        }

        // GET: Admin/Badges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Badges badges = db.Badges.Find(id);
            if (badges == null)
            {
                return HttpNotFound();
            }
            return View(badges);
        }

        // POST: Admin/Badges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BadgeID,Name,Description")] Badges badges)
        {
            if (ModelState.IsValid)
            {
                db.Entry(badges).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(badges);
        }

        // GET: Admin/Badges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Badges badges = db.Badges.Find(id);
            if (badges == null)
            {
                return HttpNotFound();
            }
            return View(badges);
        }

        // POST: Admin/Badges/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int BadgesID)
        {
            try
            {
                if (BadgesID <= 0)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var badgesID = db.Badges.Find(BadgesID);
                if (badgesID == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại!" });
                }

                db.Badges.Remove(badgesID);
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
