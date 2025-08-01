namespace ADProject.Models
{
    public class UpdateUserProfileDto
    {
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Gender { get; set; }

        public string? Url { get; set; } // 可选：用户头像或个人网站链接

        public List<int> TagIds { get; set; } // 精简成 ID 列表
    }

}
