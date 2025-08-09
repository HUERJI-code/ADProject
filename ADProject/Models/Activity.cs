using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ADProject.Models
{
    [Table("activities")]
    public class Activity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public string Status { get; set; } = "pending";  // pending / approved / rejected / cancelled
        public string? Url { get; set; } = string.Empty; // 活动照片链接

        public int number { get; set; } // 活动最多人数

        public int CreatedBy { get; set; }
        [JsonIgnore]
        public virtual User Creator { get; set; }

        public virtual List<Tag> Tags { get; set; } = new();
        [JsonIgnore]
        public virtual List<User> RegisteredUsers { get; set; } = new();
        [JsonIgnore]

        public virtual List<User> FavouritedByUsers { get; set; } = new();

    }


}
