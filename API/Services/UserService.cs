using API.IServices;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.DTO;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> GetUserInfo(int idUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == idUser);
            if (user == null)
            {
                return new ApiResponse<object>(false, null, "Người dùng không tồn tại.");
            }

            var result = new
            {
                user.UserID,
                user.Email,
                user.Username,
                user.Role
            };

            return new ApiResponse<object>(true, result, "Lấy thông tin người dùng thành công.");
        }
    }
}
