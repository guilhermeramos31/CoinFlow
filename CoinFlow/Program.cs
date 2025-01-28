using CoinFlow.Infrastructure.Configurations;
using CoinFlow.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Connection whit DB
builder.Services.AddDbContext<CoinFlowContext>( options =>
    options.UseNpgsql( builder.Configuration.GetConnectionString( "DefaultConnection" ) ) );

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.BuildSwagger(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger(configuration);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
