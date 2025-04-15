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
    public class TopicsController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // GET: Admin/Topics
        public ActionResult Index()
        {
            var topics = db.Topic.Include(t => t.PrerequisiteTopic).ToList();
            return View(topics);
        }

        // GET: Admin/Topics/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topic.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Admin/Topics/Create
        public ActionResult Create()
        {
            // Lấy danh sách các Topic đã được chọn làm prerequisite (không được chọn lại)
            var usedPrerequisiteIds = db.Topic
                                        .Where(t => t.PrerequisiteTopicId.HasValue)
                                        .Select(t => t.PrerequisiteTopicId.Value)
                                        .ToList();

            // Lấy các topic còn lại chưa được dùng làm prerequisite
            var availableTopics = db.Topic
                                    .Where(t => !usedPrerequisiteIds.Contains(t.TopicID))
                                    .ToList();

            ViewBag.PrerequisiteTopicId = new SelectList(availableTopics, "TopicID", "TopicName");

            return View();
        }
        public ActionResult GetTopicsWithStatus(int userId)
        {
            var topics = db.Topic
                .Select(t => new
                {
                    t.TopicID,
                    t.TopicName,
                    t.PrerequisiteTopicId,
                    Completed = db.UserTopic.Any(ut => ut.UserID == userId && ut.TopicID == t.TopicID && ut.Completed)
                }).ToList();

            return Json(topics, JsonRequestBehavior.AllowGet);
        }
        // POST: Admin/Topics/Create
        // POST: Admin/Topics/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "TopicID,TopicName,PrerequisiteTopicId")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                // Đảm bảo rằng TopicID được gán đúng
                topic.TopicID = Guid.NewGuid();

                // Thêm topic vào cơ sở dữ liệu
                db.Topic.Add(topic);
                db.SaveChanges();

                // Chuyển hướng về trang Index sau khi lưu
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, quay lại trang tạo với dữ liệu đã nhập
            return View(topic);
        }


        // GET: Admin/Topics/Edit
        public ActionResult Edit(Guid id)
        {
            var topic = db.Topic.Find(id);

            if (topic == null)
            {
                return HttpNotFound();
            }

            // Nếu có PrerequisiteTopicId, lấy tên của topic liên quan
            if (topic.PrerequisiteTopicId.HasValue)
            {
                var prerequisiteTopic = db.Topic
                                          .FirstOrDefault(t => t.TopicID == topic.PrerequisiteTopicId.Value);
                if (prerequisiteTopic != null)
                {
                    // Truyền tên của topic yêu cầu trước vào ViewBag
                    ViewBag.PrerequisiteTopicName = prerequisiteTopic.TopicName;
                }
            }

            return View(topic);
        }


        // POST: Admin/Topics/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicID,TopicName,PrerequisiteTopicId")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu có lỗi thì load lại danh sách dropdown
            var usedPrerequisiteIds = db.Topic
                .Where(t => t.PrerequisiteTopicId.HasValue && t.TopicID != topic.TopicID)
                .Select(t => t.PrerequisiteTopicId.Value)
                .ToList();

            var availableTopics = db.Topic
                .Where(t => !usedPrerequisiteIds.Contains(t.TopicID))
                .ToList();

            ViewBag.PrerequisiteTopicId = new SelectList(availableTopics, "TopicID", "TopicName", topic.PrerequisiteTopicId);

            // Lấy tên chủ đề liên quan nếu có
            if (topic.PrerequisiteTopicId.HasValue)
            {
                var prerequisiteTopic = db.Topic.Find(topic.PrerequisiteTopicId.Value);
                ViewBag.PrerequisiteTopicName = prerequisiteTopic != null ? prerequisiteTopic.TopicName : "Không xác định";
            }
            else
            {
                ViewBag.PrerequisiteTopicName = "Không có";
            }

            return View(topic);
        }


        // GET: Admin/Topics/Delete/5
        // GET: Admin/Topics/Delete
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topic.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topics/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(Guid TopicID)
        {
            try
            {
                if (TopicID == Guid.Empty)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var topic = db.Topic.Find(TopicID);
                if (topic == null)
                {
                    return Json(new { success = false, message = "Chủ đề không tồn tại!" });
                }

                db.Topic.Remove(topic);
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
