using API.IServices;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : BaseController
    {
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var response = await _lessonService.GetAllLesson();
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy dữ liệu: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("detail/{idLesson}")]
        public async Task<ActionResult> GetDetail(int idLesson)
        {
            try
            {
                var response = await _lessonService.GetLessonById(idLesson);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy dữ liệu: {ex.Message}");
            }
        }


        [Authorize]
        [HttpGet("complete/{idLesson}")]
        public async Task<ActionResult> CompleteLesson(int idLesson)
        {
            try
            {
                if (!UserId.HasValue)
                {
                    return Unauthorized("Không thể xác định người dùng từ token.");
                }
                var response = await _lessonService.CompleteLesson(idLesson, (int)UserId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy dữ liệu: {ex.Message}");
            }
        }
    }
}
