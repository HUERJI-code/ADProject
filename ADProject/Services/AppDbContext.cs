using Microsoft.EntityFrameworkCore;
using ADProject.Models;

namespace ADProject.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ✅ 最少改动：仅在“外部未注入”时，才使用本地默认连接（不影响测试的 InMemory & 生产的 Azure）
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var local = "Server=localhost;Database=adproject;User=root;Password=;";
                optionsBuilder.UseMySql(local, ServerVersion.AutoDetect(local));
                optionsBuilder.UseLazyLoadingProxies();
            }
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

        public DbSet<InviteCode> inviteCodes { get; set; } // 添加 InviteCode DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 表映射
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

            // 关系（保持你原有配置）
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

            // 多对多：中间表
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.Tags)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "userprofiletag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    l => l.HasOne<UserProfile>().WithMany().HasForeignKey("UserProfileId"),
                    j => { j.ToTable("userprofiletag"); j.HasKey("UserProfileId", "TagId"); });

            modelBuilder.Entity<Channel>()
                .HasMany(c => c.Tags)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "channeltag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    l => l.HasOne<Channel>().WithMany().HasForeignKey("ChannelId"),
                    j => { j.ToTable("channeltag"); j.HasKey("ChannelId", "TagId"); });

            modelBuilder.Entity<Activity>()
                .HasMany(a => a.Tags)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "activitytag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    l => l.HasOne<Activity>().WithMany().HasForeignKey("ActivityId"),
                    j => { j.ToTable("activitytag"); j.HasKey("ActivityId", "TagId"); });

            modelBuilder.Entity<Activity>()
                .HasMany(a => a.RegisteredUsers)
                .WithMany(u => u.RegisteredActivities)
                .UsingEntity<Dictionary<string, object>>(
                    "activityuser",
                    r => r.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    l => l.HasOne<Activity>().WithMany().HasForeignKey("ActivityId"),
                    j => { j.ToTable("activityuser"); j.HasKey("ActivityId", "UserId"); });

            modelBuilder.Entity<User>()
                .HasMany(u => u.favouriteActivities)
                .WithMany(a => a.FavouritedByUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "userfavouriteactivity",
                    r => r.HasOne<Activity>().WithMany().HasForeignKey("ActivityId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => { j.ToTable("userfavouriteactivity"); j.HasKey("UserId", "ActivityId"); });

            modelBuilder.Entity<Channel>()
                .HasMany(c => c.Members)
                .WithMany(u => u.Channels)
                .UsingEntity<Dictionary<string, object>>(
                    "channeluser",
                    r => r.HasOne<User>().WithMany().HasForeignKey("MembersUserId"),
                    l => l.HasOne<Channel>().WithMany().HasForeignKey("ChannelsChannelId"),
                    j => { j.ToTable("channeluser"); j.HasKey("ChannelsChannelId", "MembersUserId"); });
        }
    }
}
