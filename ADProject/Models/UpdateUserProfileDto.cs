namespace ADProject.Models
{
    public class UpdateUserProfileDto
    {
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Gender { get; set; }

        public List<int> TagIds { get; set; } // 精简成 ID 列表
    }

}
