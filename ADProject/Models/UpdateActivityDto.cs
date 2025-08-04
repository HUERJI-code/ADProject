namespace ADProject.Models
{
    public class UpdateActivityDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public int number { get; set; } // 可选，更新活动人数限制

        public string? Url { get; set; } // 可选，更新活动照片链接，默认为空
        public List<int> TagIds { get; set; }  // 可选，更新标签
    }

}
