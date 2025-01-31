using System.IdentityModel.Tokens.Jwt;
using CoinFlow.Infrastructure.Configurations;
using CoinFlow.Infrastructure.Configurations.Settings;
using CoinFlow.Infrastructure.Data.Contexts;
using CoinFlow.Infrastructure.Managers;
using CoinFlow.Models.Profiles;
using CoinFlow.Services;
using CoinFlow.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddScoped<UowManager>();
builder.Services.AddScoped<JwtSetting>();
builder.Services.AddSingleton<JwtSecurityTokenHandler>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWalletService, WalletService>();

// Connection whit DB
builder.Services.AddDbContext<CoinFlowContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.BuildSwagger(configuration);

//Identity
builder.Services.BuildIdentity();

//Auth
builder.Services.BuildAuth(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger(configuration);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.CreateRolesAsync();

app.Run();
