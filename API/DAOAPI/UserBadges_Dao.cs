using API.Model;
using Model.EF;

namespace API.DAOAPI
{
    public class UserBadges_Dao
    {
        private readonly ApplicationDbContext db;
        public UserBadges_Dao(ApplicationDbContext context)
        {
            db = context;
        }

        // 1️⃣ Lấy danh sách tất cả huy hiệu của người dùng
        public List<UserBadges> GetAllUserBadges()
        {
            return db.UserBadges.ToList();
        }

        // 2️⃣ Lấy danh sách huy hiệu của một người dùng
        public List<UserBadges> GetUserBadgesByUserId(int userId)
        {
            return db.UserBadges.Where(ub => ub.UserID == userId).ToList();
        }

        // 3️⃣ Gán huy hiệu cho người dùng
        public bool AssignBadgeToUser(int userId, int badgeId)
        {
            try
            {
                UserBadges userBadge = new UserBadges
                {
                    UserID = userId,
                    BadgeID = badgeId,
                    AwardedAt = DateTime.Now
                };

                db.UserBadges.Add(userBadge);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 4️⃣ Kiểm tra xem người dùng đã có huy hiệu chưa
        public bool CheckIfUserHasBadge(int userId, int badgeId)
        {
            return db.UserBadges.Any(ub => ub.UserID == userId && ub.BadgeID == badgeId);
        }

        // 5️⃣ Xóa huy hiệu của người dùng
        public bool RemoveUserBadge(int userBadgeId)
        {
            try
            {
                var userBadge = db.UserBadges.Find(userBadgeId);
                if (userBadge == null) return false;

                db.UserBadges.Remove(userBadge);
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
