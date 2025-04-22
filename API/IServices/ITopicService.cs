using Model.DTO;

namespace API.IServices
{
    public interface ITopicService
    {
        Task<ApiResponse<object>> GetAllTopics(int userId);
        Task<ApiResponse<object>> GetTopicById(Guid topicId);
    }
}
