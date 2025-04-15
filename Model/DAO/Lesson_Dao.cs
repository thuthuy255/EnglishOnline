using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Model.EF;

namespace Model.DAO
{
    public class LessonDAO
    {
        private EnglishOnlineDbContext _context;

        public LessonDAO(EnglishOnlineDbContext context)
        {
            _context = context;
        }

        // Lấy thông tin Lesson theo LessonID
        public Lessons GetLessonById(int lessonId)
        {
            return _context.Lessons
                           .Include(l => l.Questions) // Bao gồm thông tin về Questions
                           .FirstOrDefault(l => l.LessonID == lessonId);
        }

        // Thêm Lesson mới
        public void AddLesson(Lessons lesson)
        {
            _context.Lessons.Add(lesson);
            _context.SaveChanges();
        }

        // Cập nhật Lesson
        public void UpdateLesson(Lessons lesson)
        {
            _context.Entry(lesson).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Xóa Lesson
        public void DeleteLesson(int lessonId)
        {
            var lesson = _context.Lessons.Find(lessonId);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();
            }
        }
    }

}
