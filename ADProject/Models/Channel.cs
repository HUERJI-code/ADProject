﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ADProject.Models
{
    public class Channel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChannelId { get; set; }
        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public string? Url { get; set; } = string.Empty; // 频道照片链接

        public string status { get; set; }  // active / archived / deleted
        public virtual User Creator { get; set; }

        public string description { get; set; } = string.Empty; // 频道描述

        public virtual List<User> Members { get; set; } = new();
        public virtual List<ChannelMessage> Messages { get; set; } = new();  // 👈 新增

        public virtual List<Tag> Tags { get; set; } = new();  // 频道标签
    }
}
