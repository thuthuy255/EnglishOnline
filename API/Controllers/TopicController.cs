using Microsoft.AspNetCore.Mvc;
using API.DAOAPI;
using Model.EF;
using API.IServices;
using Microsoft.AspNetCore.Authorization;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : BaseController
    {
        private readonly ITopicService _topicService;
        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAll()
        {
            try
            {
                if (!UserId.HasValue)
                {
                    return Unauthorized("Không thể xác định người dùng từ token.");
                }
                var response = await _topicService.GetAllTopics(UserId.Value);
                if (response.Success)
                {
                    return Ok(response.Data);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy dữ liệu: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("{topicId}")]
        public async Task<ActionResult<Topic>> GetDetail(Guid topicId)
        {
            try
            {
                var response = await _topicService.GetTopicById(topicId);
                if (response.Success)
                {
                    return Ok(response.Data);
                }
                return NotFound(response.Message); // Trả về không tìm thấy
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy dữ liệu: {ex.Message}");
            }
        }
    }
}

