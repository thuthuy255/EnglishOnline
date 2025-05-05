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
                var allTopics = await _context.Topics
                    .Include(t => t.Lessons)
                    .OrderBy(t => t.TopicID)
                    .ToListAsync();
                // Lấy ra list topic

                var allProgresses = await _context.UserProgresses
                    .Where(p => p.UserID == userId)
                    .ToListAsync();

                // lấy ra tiến trình người học

                var progressesLookup = allProgresses
                 .Where(p => p.Completed)
                 .ToLookup(p => p.LessonID);

                var listTopics = allTopics.Select((topic, index) =>
                {
                    var lessons = topic.Lessons.ToList();
                    var totalLesson = lessons.Count;
                    var completedLesson = lessons.Count(lesson => progressesLookup.Contains(lesson.LessonID));

                    bool isUnlocked = true;

                    // Nếu topic không có bài học => luôn lock
                    if (totalLesson == 0)
                    {
                        isUnlocked = false;
                    }
                    else if (index > 0)
                    {
                        var previousTopic = allTopics[index - 1];
                        var previousLessons = previousTopic.Lessons.ToList();
                        var previousTotal = previousLessons.Count;
                        var previousCompleted = previousLessons.Count(lesson => progressesLookup.Contains(lesson.LessonID));

                        isUnlocked = previousCompleted == previousTotal;
                    }

                    return new
                    {
                        TopicId = topic.TopicID,
                        TopicName = topic.TopicName,
                        IsLocked = isUnlocked ? "unlock" : "lock",
                        Lessons = lessons.Select(lesson => new
                        {
                            LessonId = lesson.LessonID,
                            LessonName = lesson.LessonName,
                        }).ToList()
                    };
                }).ToList();

                return new ApiResponse<object>(true, listTopics, "Lấy thành công danh sách chủ đề");
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
                  .Where(p => p.TopicID == topicId)
                  .Select(t => new
                  {
                      TopicId = t.TopicID,
                      TopicName = t.TopicName,
                      Lesson = t.Lessons.Select(l => new
                      {
                          LessonId = l.LessonID,
                          LessonName = l.LessonName,
                          LessonContent = l.LessonContent,
                          LessonType = l.LessonType,
                          DifficultyLevel = l.DifficultyLevel
                      })
                  })
                  .ToListAsync();
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
