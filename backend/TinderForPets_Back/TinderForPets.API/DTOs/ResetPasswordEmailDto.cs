﻿namespace TinderForPets.API.DTOs
{
    public class ResetPasswordEmailDto
    {
        public string UserName { get; set; } = string.Empty;
        public string ResetLink { get; set; } = string.Empty;
    }
}