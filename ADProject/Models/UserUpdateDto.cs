﻿namespace ADProject.Models
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string NewPasswordHash { get; set; }
    }
}
