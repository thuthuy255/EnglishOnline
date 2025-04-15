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
    public class UserBadgesController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // GET: Admin/UserBadges
        public ActionResult Index()
        {
            var userBadges = db.UserBadges.Include(u => u.Badge).Include(u => u.User);
            return View(userBadges.ToList());
        }

        // GET: Admin/UserBadges/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserBadges userBadges = db.UserBadges.Find(id);
        //    if (userBadges == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userBadges);
        //}

        // GET: Admin/UserBadges/Create
        public ActionResult Create()
        {
            ViewBag.BadgeID = new SelectList(db.Badges, "BadgeID", "Name");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Admin/UserBadges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserBadgeID,UserID,BadgeID,AwardedAt")] UserBadges userBadges)
        {
            if (ModelState.IsValid)
            {
                db.UserBadges.Add(userBadges);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BadgeID = new SelectList(db.Badges, "BadgeID", "Name", userBadges.BadgeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", userBadges.UserID);
            return View(userBadges);
        }

        // GET: Admin/UserBadges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBadges userBadges = db.UserBadges.Find(id);
            if (userBadges == null)
            {
                return HttpNotFound();
            }
            ViewBag.BadgeID = new SelectList(db.Badges, "BadgeID", "Name", userBadges.BadgeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", userBadges.UserID);
            return View(userBadges);
        }

        // POST: Admin/UserBadges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserBadgeID,UserID,BadgeID,AwardedAt")] UserBadges userBadges)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userBadges).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BadgeID = new SelectList(db.Badges, "BadgeID", "Name", userBadges.BadgeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", userBadges.UserID);
            return View(userBadges);
        }

        // GET: Admin/UserBadges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBadges userBadges = db.UserBadges.Find(id);
            if (userBadges == null)
            {
                return HttpNotFound();
            }
            return View(userBadges);
        }

        // POST: Admin/UserBadges/Delete/5
        [HttpPost]
        public ActionResult DeleteUserBadgeConfirmed(int UserBadgeID)
        {
            try
            {
                if (UserBadgeID <= 0)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var badgesID = db.UserBadges.Find(UserBadgeID);
                if (badgesID == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy UserBadge!" });
                }

                db.UserBadges.Remove(badgesID);
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
