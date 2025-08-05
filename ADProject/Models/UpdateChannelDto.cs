namespace ADProject.Models
{
    public class UpdateChannelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Url { get; set; } // 频道照片链接，默认为空
        public string Description
        {
            get; set;
        }

        public List<int> TagIds { get; set; }
    }
}
