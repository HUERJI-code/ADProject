using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
        public class UserProfile
        {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

            public int UserId { get; set; }
            public virtual User User { get; set; }

            public virtual List<Tag> Tags { get; set; } = new();

            public int? Age { get; set; }
            public double? Height { get; set; }  // 单位：cm
            public double? Weight { get; set; }  // 单位：kg

            public string? Gender { get; set; }  // 可选：male / female / other
        }

}
