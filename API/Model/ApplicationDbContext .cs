using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Model.EF;
namespace API.Model
{
        public class ApplicationDbContext : DbContext
        {
            // Constructor: Khởi tạo DbContext với cấu hình chuỗi kết nối
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            { }

        // Định nghĩa DbSet cho các bảng trong cơ sở dữ liệu
        public DbSet<Users> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Lessons> Lessons { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<UserTopic> UserTopic { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<Badges> Badges { get; set; }
        public DbSet<UserBadges> UserBadges { get; set; }
    }
    
}
