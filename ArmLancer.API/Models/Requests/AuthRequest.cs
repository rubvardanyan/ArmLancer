﻿using System.ComponentModel.DataAnnotations;

namespace ArmLancer.API.Models.Requests
{
    public class AuthRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}