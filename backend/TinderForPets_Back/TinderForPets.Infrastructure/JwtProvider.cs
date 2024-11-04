using Microsoft.AspNetCore.Http;
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
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _validationParameters;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _tokenHandler = new JwtSecurityTokenHandler();

            _validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
            };
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
            try
            {
                var claimPrincipal = _tokenHandler.ValidateToken(token, _validationParameters, out SecurityToken validatedToken);
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

        public Result<Guid> ValidateAuthTokenAndExtractUserId(HttpContext context) 
        {
            try
            {
                var token = context.Request.Cookies["AuthToken"];
                var claimPrincipal = _tokenHandler.ValidateToken(token, _validationParameters, out SecurityToken validatedToken);
                if (claimPrincipal == null)
                {
                    return Result.Failure<Guid>(JwtErrors.JwtTokenExpired);
                }

                var userId = claimPrincipal.FindFirst("userId").Value;

                return Result.Success<Guid>(Guid.Parse(userId));
            }
            catch (SecurityTokenExpiredException)
            {
                return Result.Failure<Guid>(JwtErrors.JwtTokenExpired);
            }
            catch (SecurityTokenException)
            {
                return Result.Failure<Guid>(JwtErrors.InvalidToken);
            }
        }
    }
}