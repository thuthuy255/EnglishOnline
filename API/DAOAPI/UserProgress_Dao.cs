using API.Model;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<UserProgress>> GetAllUserProgressAsync()
        {
            return await db.UserProgresses.ToListAsync();
        }

        // 1️⃣ Lấy tổng điểm của người học
        public async Task<int> GetScoreByIDUser(int idUser)
        {
            var getScore = await db.UserProgresses.Where(p => p.UserID == idUser).SumAsync(p=>p.Score);
            return getScore;
        }

        // 2️⃣ Lấy tiến độ học theo ID
        public async Task<UserProgress> GetUserProgressByIdAsync(int id)
        {
            return await db.UserProgresses.FindAsync(id);
        }

        // 3️⃣ Lấy tiến độ học của một người dùng theo UserID
        public async Task<List<UserProgress>> GetProgressByUserIdAsync(int userId)
        {
            return await db.UserProgresses
                           .Where(up => up.UserID == userId)
                           .ToListAsync();
        }

        // 4️⃣ Lấy tiến độ học của một bài học theo LessonID
        public async Task<List<UserProgress>> GetProgressByLessonIdAsync(int lessonId)
        {
            return await db.UserProgresses
                           .Where(up => up.LessonID == lessonId)
                           .ToListAsync();
        }

        // 5️⃣ Thêm tiến độ học mới
        public async Task<bool> InsertUserProgressAsync(UserProgress progress)
        {
            try
            {
                await db.UserProgresses.AddAsync(progress);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 6️⃣ Cập nhật tiến độ học
        public async Task<bool> UpdateUserProgressAsync(UserProgress progress)
        {
            try
            {
                var existingProgress = await db.UserProgresses.FindAsync(progress.ProgressID);
                if (existingProgress == null) return false;

                existingProgress.Score = progress.Score;
                existingProgress.Completed = progress.Completed;

                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 7️⃣ Xóa tiến độ học theo ID
        public async Task<bool> DeleteUserProgressAsync(int id)
        {
            try
            {
                var progress = await db.UserProgresses.FindAsync(id);
                if (progress == null) return false;

                db.UserProgresses.Remove(progress);
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
