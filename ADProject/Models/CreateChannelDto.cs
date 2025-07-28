namespace ADProject.Models
{
    public class CreateChannelDto
    {
        public string Name { get; set; }
        public string Description
        {
            get; set;
        }

        public List<int> TagIds { get; set; }
    }
}
