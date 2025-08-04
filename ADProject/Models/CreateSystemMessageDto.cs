namespace ADProject.Models
{
    public class CreateSystemMessageDto
    {
        public int ReceiverId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        // Optional: You can add more properties if needed
        // For example, you might want to include a timestamp or a priority level
    }
}
