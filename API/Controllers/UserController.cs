using API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetInfoUser")]
        public IActionResult GetUserInfo()
        {
            // Lấy idUser từ claim trong token
            var idUserClaim = User.Claims.FirstOrDefault(c => c.Type == "idUser");

            if (idUserClaim == null)
            {
                return Unauthorized("Không tìm thấy idUser trong token.");
            }

            // Convert idUser từ string sang int (hoặc kiểu dữ liệu của bạn)
            var idUser = int.Parse(idUserClaim.Value);

            // Tìm người dùng trong cơ sở dữ liệu bằng idUser
            var user = _context.Users.FirstOrDefault(u => u.UserID == idUser);

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Trả về thông tin người dùng
            return Ok(new
            {
                user.UserID,
                user.Email,
                user.Username,
                user.Role
            });
        }
    }
}
