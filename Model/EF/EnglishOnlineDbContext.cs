using System.Data.Entity;

namespace Model.EF
{
    public class EnglishOnlineDbContext : DbContext
    {
        public EnglishOnlineDbContext() : base("name=EnglishOnlineDbContext") { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Lessons> Lessons { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<UserTopic> UserTopic { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<Badges> Badges { get; set; }
        public DbSet<UserBadges> UserBadges { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //M?t User (ng??i dùng) có th? có nhi?u UserProgress (ti?n trình h?c t?p).
            modelBuilder.Entity<Users>()
                .HasMany(u => u.UserProgresses)
                .WithRequired(up => up.User)
                .HasForeignKey(up => up.UserID)
                .WillCascadeOnDelete(true);
            //M?t huy hi?u có th? ???c trao cho nhi?u ng??i dùng
            modelBuilder.Entity<Users>()
                .HasMany(u => u.UserBadges)
                .WithRequired(ub => ub.User)
                .HasForeignKey(ub => ub.UserID)
                .WillCascadeOnDelete(true);
            //M?t Lesson có th? có nhi?u Questions
            modelBuilder.Entity<Lessons>()
                .HasMany(l => l.Questions)
                .WithRequired(q => q.Lesson)
                .HasForeignKey(q => q.LessonID)
                .WillCascadeOnDelete(true);
            //M?t Question có th? có nhi?u Answers
            modelBuilder.Entity<Questions>()
                .HasMany(q => q.Answers)
                .WithRequired(a => a.Question)
                .HasForeignKey(a => a.QuestionID)
                .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<Lessons>()
                .HasMany(l => l.UserProgresses)
                .WithRequired(up => up.Lesson)
                .HasForeignKey(up => up.LessonID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Badges>()
                .HasMany(b => b.UserBadges)
                .WithRequired(ub => ub.Badge)
                .HasForeignKey(ub => ub.BadgeID)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Topic>()
                .HasMany(t => t.Lessons)
                .WithRequired(l => l.Topic)
                .HasForeignKey(l => l.TopicID)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<UserTopic>()
            .HasKey(ut => new { ut.UserID, ut.TopicID });

            modelBuilder.Entity<UserTopic>()
                .HasRequired(ut => ut.User)
                .WithMany(u => u.UserTopic)
                .HasForeignKey(ut => ut.UserID)
                .WillCascadeOnDelete(false); // Không cascade delete cho UserTopic

            modelBuilder.Entity<UserTopic>()
                .HasRequired(ut => ut.Topic)
                .WithMany(t => t.UserTopic)
                .HasForeignKey(ut => ut.TopicID)
                .WillCascadeOnDelete(false); // Không cascade delete cho UserTopic
            base.OnModelCreating(modelBuilder);
        }
    }
}
