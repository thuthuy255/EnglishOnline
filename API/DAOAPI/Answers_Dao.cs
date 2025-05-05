using API.Model;
using Model.EF;
using Microsoft.EntityFrameworkCore;

namespace API.DAOAPI
{
    public class Answers_Dao
    {
        private readonly ApplicationDbContext db;

        public Answers_Dao(ApplicationDbContext context)
        {
            db = context;
        }

        // 1️⃣ Lấy danh sách tất cả câu trả lời
        public async Task<List<Answers>> GetAllAnswersAsync()
        {
            return await db.Answers.ToListAsync();
        }

        // 2️⃣ Lấy câu trả lời theo ID
        public async Task<Answers> GetAnswerByIdAsync(int id)
        {
            return await db.Answers.FindAsync(id);
        }

        // 3️⃣ Lấy danh sách câu trả lời theo ID của câu hỏi
        public async Task<List<Answers>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await db.Answers.Where(a => a.QuestionID == questionId).ToListAsync();
        }

        // 4️⃣ Thêm câu trả lời mới
        public async Task<bool> InsertAnswerAsync(Answers answer)
        {
            try
            {
                await db.Answers.AddAsync(answer);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 5️⃣ Cập nhật câu trả lời
        public async Task<bool> UpdateAnswerAsync(Answers answer)
        {
            try
            {
                var existingAnswer = await db.Answers.FindAsync(answer.AnswerID);
                if (existingAnswer == null) return false;

                existingAnswer.AnswerText = answer.AnswerText;
                existingAnswer.IsCorrect = answer.IsCorrect;
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 6️⃣ Xóa câu trả lời theo ID
        public async Task<bool> DeleteAnswerAsync(int id)
        {
            try
            {
                var answer = await db.Answers.FindAsync(id);
                if (answer == null) return false;

                db.Answers.Remove(answer);
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
