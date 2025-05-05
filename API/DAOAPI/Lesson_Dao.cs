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
        public async Task<Lessons?> GetLessonByIdAsync(int lessonId)
        {
            var lesson = await _context.Lessons
                                       .Include(l => l.Questions)
                                       .FirstOrDefaultAsync(l => l.LessonID == lessonId);

            return lesson;
        }

        // Thêm Lesson mới
        public async Task AddLessonAsync(Lessons lesson)
        {
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
        }

        // Cập nhật Lesson
        public async Task UpdateLessonAsync(Lessons lesson)
        {
            _context.Entry(lesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Xóa Lesson
        public async Task DeleteLessonAsync(int lessonId)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
        }
    }

}
