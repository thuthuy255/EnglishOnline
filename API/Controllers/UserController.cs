using API.IServices;
using API.Model;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userServie)
        {
            _userService = userServie;
        }


        [Authorize]
        [HttpGet("GetInfoUser")]
        public async Task<IActionResult> GetUserInfo()
        {
            if (!UserId.HasValue)
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var response = await _userService.GetUserInfo(UserId.Value);

            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status404NotFound, response);
            }

            return Ok(response);
        }
    }
}
