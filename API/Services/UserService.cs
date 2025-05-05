using API.DAOAPI;
using API.IServices;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.DTO;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserDao _userDao;
        private readonly UserProgress_Dao _userProgressDao;
        public UserService(ApplicationDbContext context, UserDao userDao, UserProgress_Dao userProgressDao)
        {
            _context = context;
            _userDao = userDao;
            _userProgressDao = userProgressDao;
        }

        public async Task<ApiResponse<object>> GetUserInfo(int idUser)
        {
            if(idUser <= 0 )
            {
                return new ApiResponse<object>(false, null, "Id User là bắt buộc.");
            }
            var userInfo = await _userDao.GetDetailByIdAsync(idUser);
            if (userInfo == null)
            {
                return new ApiResponse<object>(false, null, "Người dùng không tồn tại.");
            }
                var getScore = await _userProgressDao.GetScoreByIDUser(idUser);
                var result = new
                {
                    UserID = userInfo.UserID,
                    Username = userInfo.Username,
                    Email = userInfo.Email,
                    Avatar = userInfo.Avatar,
                    Role = userInfo.Role,
                    CreatedAt = userInfo.CreatedAt,
                    UpdatedAt = userInfo.UpdatedAt,
                    Score = getScore,
                };
            return new ApiResponse<object>(true, result, "Lấy thông tin người dùng thành công.");
        }
    }
}
