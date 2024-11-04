﻿using Microsoft.AspNetCore.Http;
using SharedKernel;
using TinderForPets.Core.Models;

namespace TinderForPets.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(Guid userId);
        string GenerateResetPasswordToken(string email);
        Result<string> ValidateResetPasswordToken(string token);
        Result<Guid> ValidateAuthTokenAndExtractUserId(HttpContext context);
    }
}