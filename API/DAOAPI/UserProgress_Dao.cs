using API.Model;
using Model.EF;

namespace API.DAOAPI
{
    public class UserProgress_Dao
    {
        private readonly ApplicationDbContext db;
        public UserProgress_Dao(ApplicationDbContext context)
        {
            db = context;
        }

        // 1️⃣ Lấy danh sách tất cả tiến độ học
        public List<UserProgress> GetAllUserProgress()
        {
            return db.UserProgresses.ToList();
        }

        // 2️⃣ Lấy tiến độ học theo ID
        public UserProgress GetUserProgressById(int id)
        {
            return db.UserProgresses.Find(id);
        }

        // 3️⃣ Lấy tiến độ học của một người dùng theo UserID
        public List<UserProgress> GetProgressByUserId(int userId)
        {
            return db.UserProgresses.Where(up => up.UserID == userId).ToList();
        }

        // 4️⃣ Lấy tiến độ học của một bài học theo LessonID
        public List<UserProgress> GetProgressByLessonId(int lessonId)
        {
            return db.UserProgresses.Where(up => up.LessonID == lessonId).ToList();
        }

        // 5️⃣ Thêm tiến độ học mới
        public bool InsertUserProgress(UserProgress progress)
        {
            try
            {
                db.UserProgresses.Add(progress);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 6️⃣ Cập nhật tiến độ học
        public bool UpdateUserProgress(UserProgress progress)
        {
            try
            {
                var existingProgress = db.UserProgresses.Find(progress.ProgressID);
                if (existingProgress == null) return false;

                existingProgress.Score = progress.Score;
                existingProgress.Completed = progress.Completed;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 7️⃣ Xóa tiến độ học theo ID
        public bool DeleteUserProgress(int id)
        {
            try
            {
                var progress = db.UserProgresses.Find(id);
                if (progress == null) return false;

                db.UserProgresses.Remove(progress);
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
