using API.IServices;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.DAO;
using Model.DTO;
using System.Security.Cryptography;
using System.Text;
using static Users;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
     

        public AuthService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
      
        }

        public async Task<ApiResponse<object>> Login(LoginDto model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return new ApiResponse<object>(false, null, "Tài khoản hoặc mật khẩu không được để trống");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser == null)
            {
                return new ApiResponse<object>(false, null, "Tài khoản không tồn tại");
            }

            // Kiểm tra mật khẩu
            var hashedPassword = HashPassword(model.Password);
            if (existingUser.PasswordHash != hashedPassword)
            {
                return new ApiResponse<object>(false, null, "Mật khẩu không chính xác");
            }

            // Generate JWT token
            var tokenString = _jwtService.GenerateToken(
                existingUser.UserID.ToString(),
                existingUser.Email,
                existingUser.Role ?? "User"
            );

            var responseData = new
            {
                Token = tokenString,
                Email = existingUser.Email,
                Role = existingUser.Role
            };

            return new ApiResponse<object>(true, responseData, "Đăng nhập thành công");
        }


        public async Task<ApiResponse<object>> Register(RegisterDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) ||  string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    return new ApiResponse<object>(false, null, "Vui lòng điền đẩy đủ thông tin");
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return new ApiResponse<object>(false, null, "Mật khẩu nhập lại không khớp!");
                }
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<object>(false, null, "Email đã tồn tại!");
                }

                string hashedPassword = HashPassword(model.Password);

                Users newUser = new Users
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    Role = UserRole.User.ToString()
                };
                newUser.CreatedAt = DateTime.Now;
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return new ApiResponse<object>(true, newUser, "Đăng ký thành công! Hãy đăng nhập ngay.");
              
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(false, null,"Lỗi: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
