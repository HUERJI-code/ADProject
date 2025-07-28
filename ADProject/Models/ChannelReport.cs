using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class ChannelReport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ChannelId { get; set; }
        public virtual Channel Channel { get; set; }

        public int ReportedById { get; set; }
        public virtual User ReportedBy { get; set; }

        public string Reason { get; set; }
        public string Status { get; set; } = "pending"; // pending / reviewed / rejected
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
    }

}
