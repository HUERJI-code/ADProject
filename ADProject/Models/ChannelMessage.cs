using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class ChannelMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ChannelId { get; set; }
        public virtual Channel Channel { get; set; }

        public int PostedById { get; set; }  // 发布人
        public virtual User PostedBy { get; set; }

        public string Title { get; set; }    // 消息标题
        public string Content { get; set; }  // 消息内容（可支持HTML渲染）
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        public bool IsPinned { get; set; } = false;  // 是否置顶
        public bool IsVisible { get; set; } = true;  // 是否对频道成员可见
    }
}
