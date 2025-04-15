using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserDao
    {
        private readonly EnglishOnlineDbContext _db;

        public UserDao(EnglishOnlineDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }
        public async Task<Users> GetDetailByIdAsync(int userId)
        {
            return await _db.Users.FindAsync(userId);
        }

        public async Task<Users> AddAsync(Users user)
        {
            user.CreatedAt = DateTime.Now;
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<Users> UpdateAsync(Users user)
        {
            var existingUser = await _db.Users.FindAsync(user.UserID);
            if (existingUser == null) return null;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            await _db.SaveChangesAsync();
            return existingUser;
        }


        public async Task<bool> LoginAsync(string username, string passwordHash)
        {
            return await _db.Users.AnyAsync(x => x.Username == username && x.PasswordHash == passwordHash);
        }

        public async Task<Users> GetDetailByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
