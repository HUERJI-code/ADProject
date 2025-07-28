using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class ActivityRegistrationRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public string Status { get; set; } = "pending";  // pending / approved / rejected
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
    }

}
