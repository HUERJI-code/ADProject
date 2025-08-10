using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ADProject.Models
{
    [Table("invitecodes")]
    public class InviteCode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; } // 邀请码内容

        public Boolean isUsed { get; set; } = false; // 是否已被使用
    }
}
