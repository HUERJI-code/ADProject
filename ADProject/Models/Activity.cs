using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class Activity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "pending";  // pending / approved / rejected / cancelled
        public string? Url { get; set; } = string.Empty; // 活动照片链接

        public int CreatedBy { get; set; }
        public virtual User Creator { get; set; }

        public virtual List<Tag> Tags { get; set; } = new();
        public virtual List<User> RegisteredUsers { get; set; } = new();

        public virtual List<User> FavouritedByUsers { get; set; } = new();

    }


}
