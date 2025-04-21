using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.EF;

namespace API.DAOAPI
{
    public class LessonDAO
    {
        private readonly ApplicationDbContext _context;

        public LessonDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy thông tin Lesson theo LessonID
        public Lessons? GetLessonById(int lessonId)
        {
            var lesson = _context.Lessons
                                 .Include(l => l.Questions)
                                 .FirstOrDefault(l => l.LessonID == lessonId);

            return lesson;
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
