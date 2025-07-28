using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual List<Activity> RegisteredActivities { get; set; } = new();
        public virtual List<Channel> Channels { get; set; } = new();
        public virtual List<SystemMessage> ReceivedMessages { get; set; } = new();

        public virtual UserProfile? Profile { get; set; } 
    }



}
