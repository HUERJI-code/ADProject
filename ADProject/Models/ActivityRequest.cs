using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class ActivityRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public int ReviewedById { get; set; }
        public virtual User ReviewedBy { get; set; }

        public string requestType { get; set; } //createActivity / cancelActivity / updateActivity

        public string Status { get; set; } = "pending";  // pending / approved / rejected
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
    }

}
