using API.Model;
using Model.EF;

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
        public List<Answers> GetAllAnswers()
        {
            return db.Answers.ToList();
        }

        // 2️⃣ Lấy câu trả lời theo ID
        public Answers GetAnswerById(int id)
        {
            return db.Answers.Find(id);
        }

        // 3️⃣ Lấy danh sách câu trả lời theo ID của câu hỏi
        public List<Answers> GetAnswersByQuestionId(int questionId)
        {
            return db.Answers.Where(a => a.QuestionID == questionId).ToList();
        }

        // 4️⃣ Thêm câu trả lời mới
        public bool InsertAnswer(Answers answer)
        {
            try
            {
                db.Answers.Add(answer);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 5️⃣ Cập nhật câu trả lời
        public bool UpdateAnswer(Answers answer)
        {
            try
            {
                var existingAnswer = db.Answers.Find(answer.AnswerID);
                if (existingAnswer == null) return false;

                existingAnswer.AnswerText = answer.AnswerText;
                existingAnswer.IsCorrect = answer.IsCorrect;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 6️⃣ Xóa câu trả lời theo ID
        public bool DeleteAnswer(int id)
        {
            try
            {
                var answer = db.Answers.Find(id);
                if (answer == null) return false;

                db.Answers.Remove(answer);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
