using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.EF;

namespace API.DAOAPI
{
    public class TopicDAO
    {
        private readonly ApplicationDbContext _context;

        public TopicDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các Topic
        public List<Topic> GetAllTopics()
        {
            return _context.Topics.ToList();
        }

        // Lấy thông tin Topic theo ID
        public Topic GetTopicById(Guid topicId)
        {
            return _context.Topics
                           .Include(t => t.Lessons) // Bao gồm thông tin Lessons
                           .FirstOrDefault(t => t.TopicID == topicId);
        }

        // Thêm Topic mới
        public void AddTopic(Topic topic)
        {
            _context.Topics.Add(topic);
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
            var topic = _context.Topics.Find(topicId);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                _context.SaveChanges();
            }
        }
    }

}
