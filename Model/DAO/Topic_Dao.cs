using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class TopicDAO
    {
        private EnglishOnlineDbContext _context;

        public TopicDAO(EnglishOnlineDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các Topic
        public List<Topic> GetAllTopics()
        {
            return _context.Topic.ToList();
        }

        // Lấy thông tin Topic theo ID
        public Topic GetTopicById(Guid topicId)
        {
            return _context.Topic
                           .Include(t => t.Lessons) // Bao gồm thông tin Lessons
                           .FirstOrDefault(t => t.TopicID == topicId);
        }

        // Thêm Topic mới
        public void AddTopic(Topic topic)
        {
            _context.Topic.Add(topic);
            _context.SaveChanges();
        }

        // Cập nhật Topic
        public void UpdateTopic(Topic topic)
        {
            _context.Entry(topic).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Xóa Topic
        public void DeleteTopic(Guid topicId)
        {
            var topic = _context.Topic.Find(topicId);
            if (topic != null)
            {
                _context.Topic.Remove(topic);
                _context.SaveChanges();
            }
        }
    }

}
