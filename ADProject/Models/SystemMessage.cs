using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    [Table("systemmessages")]
    public class SystemMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }

}
