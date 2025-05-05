using API.IServices;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Model.EF;

namespace API.Services
{
    public class LessonService : ILessonService
    {
        private readonly ApplicationDbContext _context;

        public LessonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> GetAllLesson()
        {
            try
            {
                var AllLessons = await _context.Lessons.ToListAsync();
                if (AllLessons == null)
                {
                    return new ApiResponse<object>(false, null, "Không có bài học nào");
                }
                return new ApiResponse<object>(true, AllLessons, "Lấy danh sách bài học thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(false, null,"Có lỗi xảy ra");
            };
        }

        public async Task<ApiResponse<object>> GetLessonById(int idLesson)
        {
            if(idLesson <= 0 )
            {
                return new ApiResponse<object>(false, null, "idLesson là bắt buộc");
            }
            try
            {
                var lesson = await _context.Lessons
                                    .Where(l => l.LessonID == idLesson)
                                    .Select(l => new
                                    {
                                        IdLesson = l.LessonID,
                                        NameLesson = l.LessonName,
                                        TypeLesson = l.LessonType,
                                        Questions = l.Questions.Select(q => new
                                        {
                                            IdQuestion = q.QuestionID,
                                            QuestionText = q.QuestionText,
                                            Answers = q.Answers.Select(a => new
                                            {
                                                IdAnswer = a.AnswerID,
                                                AnswerText = a.AnswerText,
                                                IsCorrect = a.IsCorrect
                                            }).ToList()
                                        }).ToList()
                                    })
                                    .FirstOrDefaultAsync();

                if (lesson == null)
                {
                    return new ApiResponse<object>(false, null, "Không có bài học nào");
                }
                return new ApiResponse<object>(true, lesson, "Lấy danh sách bài học thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(false, null, "Có lỗi xảy ra");
            };
        }

        public async Task<ApiResponse<object>> CompleteLesson(int idLesson,int idUser)
        {
            if(idUser <= 0)
            {
                return new ApiResponse<object>(false, null, "idUser là bắt buộc");
            }
            if (idLesson <= 0)
            {
                return new ApiResponse<object>(false, null, "idLesson là bắt buộc");
            }
            try
            {
               var userProgress = await _context.UserProgresses.FirstOrDefaultAsync(up => up.LessonID == idLesson && up.UserID == idUser);
                if (userProgress != null)
                {
                    var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonID == idLesson);
                    if(lesson == null)
                    {
                        return new ApiResponse<object>(false, null, "Không có bài học nào");
                    }
                    userProgress.Score = userProgress.Score + lesson.Score;
                    await _context.SaveChangesAsync();
                    return new ApiResponse<object>(true, null, "Bài học đã hoàn thành");
                }
                // Nếu chưa có tiến trình cho bài học này, thêm mới
                var response = await _context.UserProgresses.AddAsync(new UserProgress
                  {
                      LessonID = idLesson,
                      UserID = idUser,
                      Completed = true
                  });
                await _context.SaveChangesAsync();
                return new ApiResponse<object>(true, null, "Hoàn thành bài học thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(false, null, "Có lỗi xảy ra");
            };
        }

    }
}
