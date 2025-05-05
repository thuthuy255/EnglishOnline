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
        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await _context.Topics.ToListAsync();
        }

        // Lấy thông tin Topic theo ID
        public async Task<Topic?> GetTopicByIdAsync(Guid topicId)
        {
            return await _context.Topics
                                 .Include(t => t.Lessons) // Bao gồm thông tin Lessons
                                 .FirstOrDefaultAsync(t => t.TopicID == topicId);
        }

        // Thêm Topic mới
        public async Task AddTopicAsync(Topic topic)
        {
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
        }

        // Cập nhật Topic
        public async Task UpdateTopicAsync(Topic topic)
        {
            _context.Entry(topic).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Xóa Topic
        public async Task DeleteTopicAsync(Guid topicId)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }
        }
    }

}
