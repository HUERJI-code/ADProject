using Microsoft.EntityFrameworkCore;
using ADProject.Models;

namespace ADProject.Services
{
    public class AppDbContext : DbContext
    {

        public AppDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=localhost;Database=adproject;User=root;Password=;",
                new MySqlServerVersion(new Version(8, 0, 39)) // Adjust version as needed
                );
            optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityRegistrationRequest> ActivityRegistrationRequests { get; set; }
        public DbSet<ActivityRequest> ActivityRequest { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelReport> ChannelReports { get; set; }
        public DbSet<SystemMessage> SystemMessages { get; set; }
        public DbSet<ChannelMessage> ChannelMessages { get; set; }

        public DbSet<ChannelRequest> channelRequest { get; set; }

        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔗 User ↔ Profile（一对一）
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            // 🔗 Profile ↔ Tags（多对多）
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.Tags)
                .WithMany() // ❗️不指定 Tag 的导航属性
                .UsingEntity<Dictionary<string, object>>(
                    "UserProfileTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<UserProfile>().WithMany().HasForeignKey("UserProfileId")
                );

            modelBuilder.Entity<Activity>()
                .HasMany(a => a.Tags)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ActivityTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<Activity>().WithMany().HasForeignKey("ActivityId")
                );



            // 🔗 Activity ↔ Creator
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Creator)
                .WithMany()
                .HasForeignKey(a => a.CreatedBy);

            // 🔗 Channel ↔ Creator
            modelBuilder.Entity<Channel>()
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy);

            // 🔗 ActivityRequest ↔ Reviewer
            modelBuilder.Entity<ActivityRequest>()
                .HasOne(r => r.ReviewedBy)
                .WithMany()
                .HasForeignKey(r => r.ReviewedById);

            // 🔗 ChannelReport ↔ Reporter
            modelBuilder.Entity<ChannelReport>()
                .HasOne(r => r.ReportedBy)
                .WithMany()
                .HasForeignKey(r => r.ReportedById);

            // 🔗 SystemMessage ↔ Receiver
            modelBuilder.Entity<SystemMessage>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId);

            // 🔗 ActivityRegistrationRequest ↔ User
            modelBuilder.Entity<ActivityRegistrationRequest>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            // 🔗 ActivityRegistrationRequest ↔ Activity
            modelBuilder.Entity<ActivityRegistrationRequest>()
                .HasOne(r => r.Activity)
                .WithMany()
                .HasForeignKey(r => r.ActivityId);

            // 🔗 ChannelMessage ↔ Channel（一对多）
            modelBuilder.Entity<ChannelMessage>()
                .HasOne(m => m.Channel)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChannelId);

            // 🔗 ChannelMessage ↔ PostedBy（一对多）
            modelBuilder.Entity<ChannelMessage>()
                .HasOne(m => m.PostedBy)
                .WithMany()
                .HasForeignKey(m => m.PostedById);
        }
    }
}
