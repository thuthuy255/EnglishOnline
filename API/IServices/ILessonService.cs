using Model.DTO;

namespace API.IServices
{
    public interface ILessonService
    {
        Task<ApiResponse<object>> GetAllLesson();
        Task<ApiResponse<object>> GetLessonById(int idLesson);

        Task<ApiResponse<object>> CompleteLesson(int idLesson, int idUser);
    }
}
