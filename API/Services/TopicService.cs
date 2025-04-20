using API.DAOAPI;
using API.IServices;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Model.EF;

namespace API.Services
{
    public class TopicService : ITopicService
    {
        private readonly TopicDAO _topicDAO;
        private readonly ApplicationDbContext _context;
        public TopicService(TopicDAO topicDAO, ApplicationDbContext context)
        {
            _topicDAO = topicDAO;
            _context = context;
        }

        public async  Task<ApiResponse<object>> GetAllTopics(int UserId)
        {
            try
            {
              var topics = await _context.Topics
                .Include(t => t.Lessons) 
                .ToListAsync();
                if (topics == null || !topics.Any())
                {
                    return new ApiResponse<object>(false, null, "Không tìm thấy chủ đề nào.");
                }

             var userProgressLessonIds = await _context.UserProgresses
                     .Where(up => up.UserID == UserId)
                     .Select(up => up.LessonID)
                     .ToListAsync();

              

                    // Chuyển dữ liệu từ Topic và Lessons sang ResponseListTopic và LessonItem
                    var response = topics.Select(topic => new ResponseListTopic
                {
                    IdTopic = topic.TopicID.ToString(),
                    NameTopic = topic.TopicName,
                    ListLessons = topic.Lessons.Select(lesson => new LessonItem
                    {
                        IdLesson = lesson.LessonID.ToString(),
                        Title = lesson.LessonName,
                        Status = "unlock"
                    }).ToList()
                }).ToList();
                return new ApiResponse<object>(true, response, "Lấy thành công danh sách chủ đề");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chủ đề từ cơ sở dữ liệu.", ex);
            }
        }

        public async Task<ApiResponse<object>> GetTopicById(Guid topicId)
        {
            try
            {
                var topic = await _context.Topics
                            .Include(t => t.Lessons)
                            .FirstOrDefaultAsync(t => t.TopicID== topicId);
                if (topic == null)
                {
                    return new ApiResponse<object>(false, null, "Không tìm thấy chủ đề với ID: " + topicId);
                }
                return new ApiResponse<object>(true, topic, "Lấy thông tin chủ đề thành công.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(false, null, "Có lỗi xảy ra khi lấy thông tin chủ đề.");
            }
        }

    }
}
