using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Google;

using static Users;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();
        private readonly UserDao _userDao;
        // GET: Account
        public AccountController()
        {
            _userDao = new UserDao(new EnglishOnlineDbContext());
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" });
                }

                // Kiểm tra xem email hoặc username có tồn tại không
                var existingUser = db.Users.FirstOrDefault(u => u.Email == email);
                if (existingUser == null)
                {
                    return Json(new { success = false, message = "Tài khoản không tồn tại!" });
                }

                // Mã hóa mật khẩu nhập vào để so sánh với mật khẩu trong database
                string hashedPassword = HashPassword(password);

                Users user = db.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == hashedPassword);

                if (user != null)
                {
                    // Thêm Role vào Claims
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        user.Email,
                        DateTime.Now,
                        DateTime.Now.AddDays(30), // Thời gian sống ticket
                        false,
                        user.Role.ToString()
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    return Json(new
                    {
                        success = true,
                        message = "Đăng nhập thành công!",
                        redirectUrl = user.Role == UserRole.Admin.ToString() ? Url.Action("Index", "Home", new { area = "Admin" }) : Url.Action("Learn", "Home")
                    });
                }

                return Json(new { success = false, message = "Sai tài khoản hoặc mật khẩu!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi" + ex });
            }
        }
       
        [AllowAnonymous]
        public ActionResult TestAuth()
        {
            return Content($"Authenticated: {User.Identity.IsAuthenticated}, Role: {User.IsInRole("Admin")}");
        }
        public ActionResult CheckRole()
        {
            var user = User.Identity;
            if (user.IsAuthenticated)
            {
                var role = ((FormsIdentity)user).Ticket.UserData; // Lấy Role từ Authentication Ticket
                return Content($"User Role: {role}");
            }
            return Content("Chưa đăng nhập!");
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        // POST: Account/Register
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(string username, string email, string password, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
               string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin!" });
                }

                if (password != confirmPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu nhập lại không khớp!" });
                }

                if (await _userDao.GetDetailByEmail(email) != null)
                {
                    return Json(new { success = false, message = "Email đã tồn tại!" });
                }

                string hashedPassword = HashPassword(password);

                var newUser = new Users
                {
                    Username = username,
                    Email = email,
                    PasswordHash = hashedPassword,
                    Role = UserRole.User.ToString()
                };

                var createdUser = await _userDao.AddAsync(newUser);

                return Json(new
                {
                    success = true,
                    message = "Đăng ký thành công! Hãy đăng nhập ngay.",
                    data = createdUser
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi" + ex });
            }

        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            // Hủy xác thực FormsAuthentication
            FormsAuthentication.SignOut();

            // Xóa toàn bộ session
            Session.Clear();
            Session.Abandon();

            // Xóa cookie xác thực (nếu có)
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1), // Hết hạn cookie
                    Value = null
                };
                Response.Cookies.Add(authCookie);
            }

            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // OTP gồm 6 chữ số
        }
        public void SendOTPEmail(string email, string otp)
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
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                // 🛠 Tạo OTP mới
                string otp = GenerateOTP();

                // 🛠 Lưu OTP vào Session
                Session["OTP"] = otp;
                Session["OTP_Email"] = email;

                // 🛠 Gửi OTP qua email
                SendOTPEmail(email, otp);

                return Json(new { success = true, message = "OTP đã được gửi!" });
            }
            return Json(new { success = false, message = "Email không tồn tại!" });
        }
        [HttpPost]
        public JsonResult VerifyOTP(string otp)
        {
            string storedOtp = Session["OTP"] as string;
            string storedEmail = Session["OTP_Email"] as string;

            if (!string.IsNullOrEmpty(storedOtp) && storedOtp == otp)
            {
                // 🛠 Xóa OTP khỏi Session sau khi xác thực thành công
                Session.Remove("OTP");
                Session.Remove("OTP_Email");

                return Json(new { success = true, message = "Xác thực thành công!" });
            }
            return Json(new { success = false, message = "Mã OTP không đúng!" });
        }
        // Hàm băm mật khẩu SHA256
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