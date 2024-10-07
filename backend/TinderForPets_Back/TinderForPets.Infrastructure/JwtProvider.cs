using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TinderForPets.Core;
using TinderForPets.Core.Models;

namespace TinderForPets.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
 
        public string GenerateToken(User user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_options.SecretKey)),
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
        public string GenerateResetPasswordToken(string email) 
        {
            Claim[] claims = [new("email", email), new("purpose", "reset-password")];

            var signingCredentials = new SigningCredentials(
               new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(_options.SecretKey)),
                   SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(30)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }

        public Result<string> ValidateResetPasswordToken(string token) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
            };

            try
            {
                var claimPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if(claimPrincipal == null) 
                {
                    return Result.Failure<string>(JwtErrors.JwtTokenExpired);
                }

                var email = claimPrincipal.FindFirst(ClaimTypes.Email).Value;
                var purpose = claimPrincipal.FindFirst("purpose").Value;

                if (purpose != "reset-password") 
                {
                    return Result.Failure<string>(JwtErrors.JwtTokenExpired);
                }

                return Result.Success<string>(email);
            }
            catch (SecurityTokenExpiredException)
            {
                return Result.Failure<string>(JwtErrors.JwtTokenExpired);
            }
            catch (SecurityTokenException)
            {
                return Result.Failure<string>(JwtErrors.InvalidToken);
            }

        }
    }
}