using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TinderForPets.Infrastructure;

namespace TinderForPets.API.Extensions
{
    public static class ApiExtension
    {
        public static void AddApiAutentification(
            this IServiceCollection services,
            IConfiguration configuration) 
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            var googleAuthOptions = configuration.GetSection(nameof(GoogleAuthOptions)).Get<GoogleAuthOptions>();

            if (jwtOptions == null) throw new ArgumentNullException(nameof(jwtOptions));
            if(googleAuthOptions == null) throw new ArgumentNullException(nameof(googleAuthOptions));

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["AuthToken"];

                            return Task.CompletedTask;
                        }
                    };
                })
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
                    options.ClientId = googleAuthOptions.ClientId;
                    options.ClientSecret = googleAuthOptions.ClientSecret;
                });

            services.AddAuthorization();
        
        }
    }
}
