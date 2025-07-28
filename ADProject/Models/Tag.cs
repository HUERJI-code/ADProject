
using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }



}
