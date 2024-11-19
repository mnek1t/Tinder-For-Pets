using Microsoft.EntityFrameworkCore;
using TinderForPets.Data;

namespace TinderForPets.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app) 
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using TinderForPetsDbContext dbContext = scope.ServiceProvider.GetRequiredService<TinderForPetsDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
