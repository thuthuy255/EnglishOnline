using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Model.EF;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private EnglishOnlineDbContext db = new EnglishOnlineDbContext();
        private CloudinaryService _cloudinaryService = new CloudinaryService();


        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //// GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string Username, string Email, string Password, string Role, HttpPostedFileBase AvatarUpload)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users
                {
                    Username = Username,
                    Email = Email,
                    PasswordHash = HashPassword(Password), // Mã hóa mật khẩu trước khi lưu
                    Role = Role
                };

                if (AvatarUpload != null && AvatarUpload.ContentLength > 0)
                {
                    // Đảm bảo _cloudinaryService.UploadImageAsync là phương thức bất đồng bộ (async)
                    string avatarUrl = await _cloudinaryService.UploadImageAsync(AvatarUpload);
                    user.Avatar = avatarUrl; // Lưu URL của ảnh vào thuộc tính Avatar
                }

                db.Users.Add(user);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Tạo thành công!";
                return RedirectToAction("Index");
            }

            return View();
        }



        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? UserID, string Username, string Email, string NewPassword, string ConfirmPassword, string Role, HttpPostedFileBase AvatarUpload)
        {
            if (UserID == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy ID người dùng!";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "UserID không hợp lệ.");
            }

            var user = db.Users.Find(UserID);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                user.Username = Username;
                user.Email = Email;
                user.Role = Role;

                // Xử lý mật khẩu
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    if (NewPassword != ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Mật khẩu xác nhận không khớp!");
                        return View(user);
                    }

                    user.PasswordHash = HashPassword(NewPassword);
                }

                // Xử lý ảnh đại diện
                if (AvatarUpload != null && AvatarUpload.ContentLength > 0)
                {
                    try
                    {
                        // Gọi service để upload ảnh lên Cloudinary
                        var cloudService = new CloudinaryService(); // chắc chắn đã cấu hình đúng
                        var imageUrl = await cloudService.UploadImageAsync(AvatarUpload);

                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            user.Avatar = imageUrl;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tải ảnh lên thất bại!");
                            return View(user);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi tải ảnh lên: " + ex.Message);
                        return View(user);
                    }
                }

                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Sửa thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật: " + ex.Message);
                    return View(user);
                }
            }

            return View(user);
        }





        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int UserID)
        {
            if (UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(UserID);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        [HttpPost]

        public ActionResult DeleteConfirmed(int UserID)
        {
            try
            {
                if (UserID <= 0)
                {
                    return Json(new { success = false, message = "ID không hợp lệ!" });
                }

                var user = db.Users.Find(UserID);
                if (user == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại!" });
                }

                db.Users.Remove(user);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Xóa thành công!";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa: " + ex.Message });
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // Hàm mã hóa SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Chuyển byte thành chuỗi hex
                }
                return builder.ToString();
            }
        }

    }

}