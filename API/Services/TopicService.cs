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
        private readonly ApplicationDbContext _context;
        public TopicService( ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> GetAllTopics(int userId)
        {
            try
            {
                var topics = await _context.Topics
                    .Include(t => t.Lessons)
                    .OrderBy(t => t.TopicID) // Sắp xếp nếu cần
                    .ToListAsync();

                if (!topics.Any())
                    return new ApiResponse<object>(false, null, "Không tìm thấy chủ đề nào.");

                // Lấy danh sách bài học đã học của user
                var userProgressLessonIds = await _context.UserProgresses
                    .Where(up => up.UserID == userId)
                    .Select(up => up.LessonID)
                    .ToListAsync();

                var response = new List<ResponseListTopic>();

                response = topics.Select((topic, i) =>
                {
                    var lessons = topic.Lessons ?? new List<Lessons>();

                    // Nếu không có bài học nào thì trả về topic bị khoá
                    if (!lessons.Any())
                    {
                        return new ResponseListTopic
                        {
                            IdTopic = topic.TopicID.ToString(),
                            NameTopic = topic.TopicName,
                            ListLessons = new List<LessonItem>(),
                            Status = "lock"
                        };
                    }

                    // Kiểm tra topic trước đã hoàn thành hay chưa
                    bool previousCompleted = i == 0 ||
                        (topics[i - 1].Lessons?.All(l => userProgressLessonIds.Contains(l.LessonID)) == true);// ép kiểu dữ liệu về bool

                    // Kiểm tra xem topic này có được mở khoá không
                    bool isUnlocked = !topic.PrerequisiteTopicId.HasValue || previousCompleted;

                    // Tạo danh sách bài học
                    var lessonItems = lessons.Select(lesson => new LessonItem
                    {
                        IdLesson = lesson.LessonID.ToString(),
                        Title = lesson.LessonName
                    }).ToList();

                    return new ResponseListTopic
                    {
                        IdTopic = topic.TopicID.ToString(),
                        NameTopic = topic.TopicName,
                        ListLessons = lessonItems,
                        Status = isUnlocked ? "unlock" : "lock"
                    };
                }).ToList();




                return new ApiResponse<object>(true, response, "Lấy thành công danh sách chủ đề");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(false, null, $"Lỗi khi lấy danh sách chủ đề: {ex.Message}");
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
