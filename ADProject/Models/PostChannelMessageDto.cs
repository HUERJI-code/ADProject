namespace ADProject.Models
{
    public class PostChannelMessageDto
    {
        public int ChannelId { get; set; }  // 频道 ID
        public string Title { get; set; }    // 消息标题
        public string Content { get; set; }  // 支持 HTML 内容渲染
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        public bool IsPinned { get; set; } = false;  // 是否置顶
        public bool IsVisible { get; set; } = true;  // 是否频道成员可见
    }
}
