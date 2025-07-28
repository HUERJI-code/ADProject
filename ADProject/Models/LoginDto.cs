namespace ADProject.Models
{
    public class LoginDto
    {
        public string Identifier { get; set; }      // 用户名或邮箱
        public string PasswordHash { get; set; }    // 密码哈希值
    }


}
