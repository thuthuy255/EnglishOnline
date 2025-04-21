using API.Model;
using Microsoft.EntityFrameworkCore;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DAOAPI
{
    public class UserDao
    {
        private readonly ApplicationDbContext _db;
        public UserDao(ApplicationDbContext context)
        {
            _db = context;
        }

        // 1️⃣ Lấy tất cả người dùng
        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        // 2️⃣ Lấy chi tiết người dùng theo ID
        public async Task<Users?> GetDetailByIdAsync(int userId)
        {
            return await _db.Users.FindAsync(userId);
        }

        // 3️⃣ Thêm người dùng mới
        public async Task<Users> AddAsync(Users user)
        {
            user.CreatedAt = DateTime.Now;
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        // 4️⃣ Cập nhật người dùng
        public async Task<Users?> UpdateAsync(Users user)
        {
            var existingUser = await _db.Users.FindAsync(user.UserID);
            if (existingUser == null) return null;

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;

            await _db.SaveChangesAsync();
            return existingUser;
        }

        // 5️⃣ Đăng nhập - kiểm tra tài khoản và mật khẩu (hash)
        public async Task<bool> LoginAsync(string username, string passwordHash)
        {
            return await _db.Users.AnyAsync(u => u.Username == username && u.PasswordHash == passwordHash);
        }

        // 6️⃣ Lấy người dùng theo Email
        public async Task<Users?> GetDetailByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
