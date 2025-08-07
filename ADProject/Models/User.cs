using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ADProject.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public string status { get; set; } = "active";  // active / inactive / banned
        [JsonIgnore]

        public virtual List<Activity> RegisteredActivities { get; set; } = new();
        [JsonIgnore]
        public virtual List<Channel> Channels { get; set; } = new();
        public virtual List<SystemMessage> ReceivedMessages { get; set; } = new();
        [JsonIgnore]

        public virtual List<Activity> favouriteActivities { get; set; } = new();

        public virtual UserProfile? Profile { get; set; }
    }



}
