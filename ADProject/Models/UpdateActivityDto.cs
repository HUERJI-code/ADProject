namespace ADProject.Models
{
    public class UpdateActivityDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<int> TagIds { get; set; }  // 可选，更新标签
    }

}
