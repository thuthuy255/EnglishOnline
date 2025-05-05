using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.EF;

namespace API.DAOAPI
{
    public class Badges_Dao
    {
        private readonly ApplicationDbContext db;
        public Badges_Dao(ApplicationDbContext context)
        {
            db = context;
        }

        // 1️⃣ Lấy danh sách tất cả huy hiệu
        public async Task<List<Badges>> GetAllBadgesAsync()
        {
            return await db.Badges.ToListAsync();
        }

        // 2️⃣ Lấy huy hiệu theo ID
        public async Task<Badges?> GetBadgeByIdAsync(int id)
        {
            return await db.Badges.FindAsync(id);
        }

        // 3️⃣ Thêm huy hiệu mới
        public async Task<bool> InsertBadgeAsync(Badges badge)
        {
            try
            {
                await db.Badges.AddAsync(badge);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 4️⃣ Cập nhật huy hiệu
        public async Task<bool> UpdateBadgeAsync(Badges badge)
        {
            try
            {
                var existingBadge = await db.Badges.FindAsync(badge.BadgeID);
                if (existingBadge == null) return false;

                existingBadge.Name = badge.Name;
                existingBadge.Description = badge.Description;

                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 5️⃣ Xóa huy hiệu theo ID
        public async Task<bool> DeleteBadgeAsync(int id)
        {
            try
            {
                var badge = await db.Badges.FindAsync(id);
                if (badge == null) return false;

                db.Badges.Remove(badge);
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
