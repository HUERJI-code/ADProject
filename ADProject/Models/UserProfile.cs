﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
        public class UserProfile
        {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

            public int UserId { get; set; }
            public virtual User User { get; set; }

            public virtual List<Tag> Tags { get; set; } = new();

            public int? Age { get; set; }

            public string? url { get; set; }  // 可选：用户头像或个人网站链接

        public string? Gender { get; set; }  // 可选：male / female / other
        }

}
