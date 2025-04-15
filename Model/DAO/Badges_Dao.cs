using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Model.EF;

namespace Model.DAO
{
    public class Badges_Dao
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();

        // 1️⃣ Lấy danh sách tất cả huy hiệu
        public List<Badges> GetAllBadges()
        {
            return db.Badges.ToList();
        }

        // 2️⃣ Lấy huy hiệu theo ID
        public Badges GetBadgeById(int id)
        {
            return db.Badges.Find(id);
        }

        // 3️⃣ Thêm huy hiệu mới
        public bool InsertBadge(Badges badge)
        {
            try
            {
                db.Badges.Add(badge);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 4️⃣ Cập nhật huy hiệu
        public bool UpdateBadge(Badges badge)
        {
            try
            {
                var existingBadge = db.Badges.Find(badge.BadgeID);
                if (existingBadge == null) return false;

                existingBadge.Name = badge.Name;
                existingBadge.Description = badge.Description;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 5️⃣ Xóa huy hiệu theo ID
        public bool DeleteBadge(int id)
        {
            try
            {
                var badge = db.Badges.Find(id);
                if (badge == null) return false;

                db.Badges.Remove(badge);
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
