using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using TinderForPets.API.Middlewares;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;
using TinderForPets.Data;
using TinderForPets.Data.Interfaces;
using TinderForPets.Data.Repositories;
using TinderForPets.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using TinderForPets.API;
using TinderForPets.Application.Interfaces;
using TinderForPets.API.Hubs;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => 
    {
        policy.WithOrigins("https://localhost:3000");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});
// Add services to the container.
builder.Services.AddHttpClient();

// Adding Options for Configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection(nameof(AuthMessageSenderOptions)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// DataBase and data handling
builder.Services.AddDbContext<TinderForPetsDbContext>(
    options => 
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(TinderForPetsDbContext)));
    });

builder.Services.AddTransient<TinderForPetsDataSeeder>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConfiguration = builder.Configuration["RedisCacheOptions:Configuration"];
    return ConnectionMultiplexer.Connect(redisConfiguration);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
    options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
    //options.ConnectionMultiplexerFactory = async () =>
    //{
    //    return await ConnectionMultiplexer.ConnectAsync(builder.Configuration["RedisCacheOptions:Configuration"]);
    //};
});



// Mapping
builder.Services.AddAutoMapper(typeof(TinderForPets.Infrastructure.AutoMapper));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBreedRepository, BreedRepository>();
builder.Services.AddScoped<ISexRepository, SexRepository>();
builder.Services.AddScoped<IAnimalTypeRepository, AnimalTypeRepository>();
builder.Services.AddScoped<IAnimalProfileRepository, AnimalProfileRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalImageRepository, AnimalImageRepository>();

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AnimalService>();
builder.Services.AddScoped<ImageHandlerService>();
builder.Services.AddScoped<GeocodingService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<SwipeService>();
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddScoped<S2GeometryService>();


// Infrastructure
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddApiAutentification(builder.Configuration);

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

var app = builder.Build();
app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<TinderForPetsDataSeeder>();
    await seeder.SeedAsync();
}

app.MapHub<RecommendationHub>("/test-hub");
app.Run();
