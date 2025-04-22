using API.IServices;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.DAO;
using Model.DTO;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using API.DAOAPI;
using UserDao = API.DAOAPI.UserDao;
using static Users;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IDistributedCache _cache;
        private readonly UserDao _userDao;
     
        public AuthService(ApplicationDbContext context, IJwtService jwtService, IDistributedCache cache, UserDao userDao)
        {
            _context = context;
            _jwtService = jwtService;
            _cache = cache;
            _userDao = userDao;
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
        public async Task<ApiResponse<object>> CheckEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                // Tạo OTP mới
                string otp = GenerateOTP();
                // Lưu OTP vào Redis (hoặc bộ nhớ)
                var cacheKey = $"OTP_{email}";

                try
                {
                    await _cache.SetStringAsync(cacheKey, otp, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                    Console.WriteLine("OTP đã lưu vào Redis.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lưu OTP vào Redis: {ex.Message}");
                }
                SendOTPEmail(email, otp);

                return new ApiResponse<object>(true, null, "OTP đã được gửi!");
            }

            return new ApiResponse<object>(false, null, "Email không tồn tại!");
        }
        public async Task<ApiResponse<object>> VerifyOTP(string email, string otp)
        {
            // Kiểm tra OTP từ Redis (hoặc bộ nhớ)
            var cacheKey = $"OTP_{email}";
            var storedOtp = await _cache.GetStringAsync(cacheKey);

            if (storedOtp == null)
            {
                return new ApiResponse<object>(false, null, "OTP đã hết hạn hoặc không hợp lệ.");
            }

            if (storedOtp == otp)
            {
                return new ApiResponse<object>(true, null, "OTP xác thực thành công.");
        }

            return new ApiResponse<object>(false, null, "OTP không đúng.");
        }
        public async Task<ApiResponse<object>> ForgotPassword(ForgotPasswordDTO model)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.Email))
            {
                return new ApiResponse<object>(false, null, "Vui lòng nhập đầy đủ thông tin");
            }

            // Tìm người dùng trong cơ sở dữ liệu
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return new ApiResponse<object>(false, null, "Email không tồn tại!");
            }

            // Mã hóa mật khẩu mới trước khi lưu vào cơ sở dữ liệu
            string hashedPassword = HashPassword(model.NewPassword);

            // Cập nhật mật khẩu mới vào người dùng
            user.PasswordHash = hashedPassword;
            Users response = await _userDao.UpdateAsync(user);

            // Nếu không cập nhật được người dùng, trả về lỗi
            if (response == null)
            {
                return new ApiResponse<object>(false, null, "Có lỗi xảy ra khi cập nhật mật khẩu!");
            }

            // Trả về phản hồi thành công
            return new ApiResponse<object>(true, null, "Mật khẩu đã được cập nhật thành công!");
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
        public string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // OTP gồm 6 chữ số
        }
        public void SendOTPEmail(string email, string otp)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("thuychi251004@gmail.com", "b e c l f t z j l t j t f o e v"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("thuychi251004@gmail.com"),
                    Subject = "Mã xác nhận OTP",
                    Body = $"Mã OTP của bạn là: {otp}. Mã này có hiệu lực trong 5 phút.",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Ghi lại thông tin lỗi hoặc log
                Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
            }
        }
    }
}
