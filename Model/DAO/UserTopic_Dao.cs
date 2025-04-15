using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserTopicDAO
    {
        private EnglishOnlineDbContext _context;

        public UserTopicDAO(EnglishOnlineDbContext context)
        {
            _context = context;
        }

        // Lấy thông tin về UserTopic theo UserId
        public List<UserTopic> GetUserTopicsByUserId(int userId)
        {
            return _context.UserTopic
                           .Where(ut => ut.UserID == userId)
                           .Include(ut => ut.Topic)  // Bao gồm thông tin về Topic
                           .ToList();
        }

        // Thêm UserTopic mới
        public void AddUserTopic(UserTopic userTopic)
        {
            _context.UserTopic.Add(userTopic);
            _context.SaveChanges();
        }

        // Cập nhật UserTopic
        public void UpdateUserTopic(UserTopic userTopic)
        {
            _context.Entry(userTopic).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Xóa UserTopic
        public void DeleteUserTopic(int userId, Guid topicId)
        {
            var userTopic = _context.UserTopic
                                    .FirstOrDefault(ut => ut.UserID == userId && ut.TopicID == topicId);
            if (userTopic != null)
            {
                _context.UserTopic.Remove(userTopic);
                _context.SaveChanges();
            }
        }
    }

}
