
using API.Model;
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
        public List<Questions> GetAllQuestions()
        {
            return db.Questions.ToList();
        }

        // Lấy câu hỏi theo ID
        public Questions GetQuestionById(int id)
        {
            return db.Questions.Find(id);
        }

        // Lấy danh sách câu hỏi theo ID bài học
        public List<Questions> GetQuestionsByLessonId(int lessonId)
        {
            return db.Questions.Where(q => q.LessonID == lessonId).ToList();
        }

        // Thêm câu hỏi mới
        public bool InsertQuestion(Questions question)
        {
            try
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cập nhật thông tin câu hỏi
        public bool UpdateQuestion(Questions question)
        {
            try
            {
                var existingQuestion = db.Questions.Find(question.QuestionID);
                if (existingQuestion == null) return false;
                existingQuestion.QuestionText = question.QuestionText;
                existingQuestion.QuestionType = question.QuestionType;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Xóa câu hỏi theo ID
        public bool DeleteQuestion(int id)
        {
            try
            {
                var question = db.Questions.Find(id);
                if (question == null) return false;

                db.Questions.Remove(question);
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

