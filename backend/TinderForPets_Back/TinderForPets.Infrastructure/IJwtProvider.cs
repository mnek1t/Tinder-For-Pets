﻿using SharedKernel;
using TinderForPets.Core.Models;

namespace TinderForPets.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        string GenerateResetPasswordToken(string email);
        Result<string> ValidateResetPasswordToken(string token);
    }
}