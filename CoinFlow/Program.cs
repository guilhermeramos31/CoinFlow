using CoinFlow.Infrastructure.Configurations;
using CoinFlow.Infrastructure.Data.Contexts;
using CoinFlow.Models.Profiles;
using CoinFlow.Services;
using CoinFlow.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddScoped<IUserService, UserService>();

// Connection whit DB
builder.Services.AddDbContext<CoinFlowContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.BuildSwagger(configuration);

//Identity
builder.Services.BuildIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger(configuration);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.CreateRolesAsync();

app.Run();
