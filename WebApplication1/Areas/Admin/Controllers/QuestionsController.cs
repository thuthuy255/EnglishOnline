using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class QuestionsController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // GET: Admin/Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.Lesson);
            return View(questions.ToList());
        }

        // GET: Admin/Questions/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Questions questions = db.Questions.Find(id);
        //    if (questions == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(questions);
        //}

        // GET: Admin/Questions/Create
        public ActionResult Create()
        {
            var topics = db.Topic.Include(t => t.PrerequisiteTopic).ToList();
            ViewBag.listTopic = topics;
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName");
            return View();
        }
        public async Task<JsonResult> GetLessonByTopicID(Guid TopicID)
        {
            var listLesson = await db.Lessons
                                     .Where(t => t.TopicID == TopicID)
                                     .Select(l => new
                                     {
                                         l.LessonID,
                                         l.LessonName
                                     })
                                     .ToListAsync();

            return Json(listLesson, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionID,LessonID,QuestionText,QuestionType")] Questions questions)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(questions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", questions.LessonID);
            return View(questions);
        }

        // GET: Admin/Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = db.Questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", questions.LessonID);
            return View(questions);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionID,LessonID,QuestionText,QuestionType")] Questions questions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", questions.LessonID);
            return View(questions);
        }

        // GET: Admin/Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = db.Questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            }
            return View(questions);
        }

        // POST: Admin/Questions/Delete/5
        public ActionResult DeleteConfirmed(int QuestionID)
        {
            try
            {
                if (QuestionID <= 0)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var question = db.Questions.Find(QuestionID);
                if (question == null)
                {
                    return Json(new { success = false, message = "Câu hỏi không tồn tại!" });
                }

                db.Questions.Remove(question);
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
