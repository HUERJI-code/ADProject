namespace ADProject.Models
{
    public class UpdateActivityDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string? Url { get; set; } // 可选，更新活动照片链接，默认为空
        public List<int> TagIds { get; set; }  // 可选，更新标签
    }

}
