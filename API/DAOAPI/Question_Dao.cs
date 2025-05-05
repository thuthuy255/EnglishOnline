
using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.EF;

namespace API.DAOAPI
{
    public class Question_Dao
    {
        private readonly ApplicationDbContext db;
        public Question_Dao(ApplicationDbContext context)
        {
            db = context;
        }

        // Lấy danh sách tất cả câu hỏi
        public async Task<List<Questions>> GetAllQuestionsAsync()
        {
            return await db.Questions.ToListAsync();
        }

        // Lấy câu hỏi theo ID
        public async Task<Questions?> GetQuestionByIdAsync(int id)
        {
            return await db.Questions.FindAsync(id);
        }

        // Lấy danh sách câu hỏi theo ID bài học
        public async Task<List<Questions>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            return await db.Questions
                           .Where(q => q.LessonID == lessonId)
                           .ToListAsync();
        }

        // Thêm câu hỏi mới
        public async Task<bool> InsertQuestionAsync(Questions question)
        {
            try
            {
                await db.Questions.AddAsync(question);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cập nhật thông tin câu hỏi
        public async Task<bool> UpdateQuestionAsync(Questions question)
        {
            try
            {
                var existingQuestion = await db.Questions.FindAsync(question.QuestionID);
                if (existingQuestion == null) return false;

                existingQuestion.QuestionText = question.QuestionText;
                existingQuestion.QuestionType = question.QuestionType;

                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Xóa câu hỏi theo ID
        public async Task<bool> DeleteQuestionAsync(int id)
        {
            try
            {
                var question = await db.Questions.FindAsync(id);
                if (question == null) return false;

                db.Questions.Remove(question);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

