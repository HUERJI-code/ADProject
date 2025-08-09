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
            // ==== 显式指定实体表名（与你库 SHOW TABLES 一致，全小写）====
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<UserProfile>().ToTable("userprofiles");
            modelBuilder.Entity<Tag>().ToTable("tags");
            modelBuilder.Entity<Activity>().ToTable("activities");
            modelBuilder.Entity<ActivityRegistrationRequest>().ToTable("activityregistrationrequests");
            modelBuilder.Entity<ActivityRequest>().ToTable("activityrequest");
            modelBuilder.Entity<Channel>().ToTable("channels");
            modelBuilder.Entity<ChannelReport>().ToTable("channelreports");
            modelBuilder.Entity<SystemMessage>().ToTable("systemmessages");
            modelBuilder.Entity<ChannelMessage>().ToTable("channelmessages");
            modelBuilder.Entity<ChannelRequest>().ToTable("channelrequest");

            // ==== 一对一 / 一对多（保持你原来的关系不变）====
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Creator)
                .WithMany()
                .HasForeignKey(a => a.CreatedBy);

            modelBuilder.Entity<Channel>()
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy);

            modelBuilder.Entity<ActivityRequest>()
                .HasOne(r => r.ReviewedBy).WithMany()
                .HasForeignKey(r => r.ReviewedById);

            modelBuilder.Entity<ChannelReport>()
                .HasOne(r => r.ReportedBy).WithMany()
                .HasForeignKey(r => r.ReportedById);

            modelBuilder.Entity<SystemMessage>()
                .HasOne(m => m.Receiver).WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId);

            modelBuilder.Entity<ActivityRegistrationRequest>()
                .HasOne(r => r.User).WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<ActivityRegistrationRequest>()
                .HasOne(r => r.Activity).WithMany()
                .HasForeignKey(r => r.ActivityId);

            modelBuilder.Entity<ChannelMessage>()
                .HasOne(m => m.Channel).WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChannelId);

            modelBuilder.Entity<ChannelMessage>()
                .HasOne(m => m.PostedBy).WithMany()
                .HasForeignKey(m => m.PostedById);

            // ==== 多对多：显式指定“中间表”名字 + 复合主键 ====

            // UserProfile ↔ Tag  ->  userprofiletag
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.Tags)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "userprofiletag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    l => l.HasOne<UserProfile>().WithMany().HasForeignKey("UserProfileId"),
                    j => { j.ToTable("userprofiletag"); j.HasKey("UserProfileId", "TagId"); });

            // Channel ↔ Tag  ->  channeltag
            modelBuilder.Entity<Channel>()
                .HasMany(c => c.Tags)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "channeltag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    l => l.HasOne<Channel>().WithMany().HasForeignKey("ChannelId"),
                    j => { j.ToTable("channeltag"); j.HasKey("ChannelId", "TagId"); });

            // Activity ↔ Tag  ->  activitytag
            modelBuilder.Entity<Activity>()
                .HasMany(a => a.Tags)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "activitytag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    l => l.HasOne<Activity>().WithMany().HasForeignKey("ActivityId"),
                    j => { j.ToTable("activitytag"); j.HasKey("ActivityId", "TagId"); });

            // Activity ↔ User(报名/参加)  ->  activityuser
            modelBuilder.Entity<Activity>()
                .HasMany(a => a.RegisteredUsers)
                .WithMany(u => u.RegisteredActivities)
                .UsingEntity<Dictionary<string, object>>(
                    "activityuser",
                    r => r.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    l => l.HasOne<Activity>().WithMany().HasForeignKey("ActivityId"),
                    j => { j.ToTable("activityuser"); j.HasKey("ActivityId", "UserId"); });

            // User ↔ Activity(收藏)  ->  userfavouriteactivity
            modelBuilder.Entity<User>()
                .HasMany(u => u.favouriteActivities)
                .WithMany(a => a.FavouritedByUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "userfavouriteactivity",
                    r => r.HasOne<Activity>().WithMany().HasForeignKey("ActivityId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => { j.ToTable("userfavouriteactivity"); j.HasKey("UserId", "ActivityId"); });

            // 如果你还有 Channel ↔ User(成员)  ->  channeluser
            // 视你的实体是否有导航属性，如果有，按下面模式补一段：
            // Channel ↔ User 成员关系 -> channeluser
            modelBuilder.Entity<Channel>()
                .HasMany(c => c.Members)               // 你的导航属性名
                .WithMany(u => u.Channels)       // 你的导航属性名
                .UsingEntity<Dictionary<string, object>>(
                    "channeluser",
                    r => r.HasOne<User>()
                          .WithMany()
                          .HasForeignKey("MembersUserId"),      // ← 按库里真实列名
                    l => l.HasOne<Channel>()
                          .WithMany()
                          .HasForeignKey("ChannelsChannelId"),  // ← 按库里真实列名
                    j =>
                    {
                        j.ToTable("channeluser");
                        j.HasKey("ChannelsChannelId", "MembersUserId");
                    });

        }

    }
}
