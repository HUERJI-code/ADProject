using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class ChannelrEQUEST
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrganizerId { get; set; }
        public virtual User User { get; set; }

        public int ChannelId { get; set; }
        public virtual Channel channel { get; set; }

        public string Status { get; set; } = "pending";  // pending / approved / rejected
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }

    }
}
