using CoinFlow.Infrastructure.Configurations.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace CoinFlow.Infrastructure.Configurations;

public static class SwaggerConfig
{
    private static void SwaggerGen(IServiceCollection service, IConfiguration configuration)
    {
        var swaggerSettings = Utils.Settings.GetSettings<SwaggerSetting>("Swagger", configuration);
        service.AddSwaggerGen(options =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });

            options.SwaggerDoc(swaggerSettings.Version,
                new OpenApiInfo
                {
                    Title = swaggerSettings.Title, Version = swaggerSettings.Version,
                    Description = swaggerSettings.Description
                });
        });
    }

    public static void BuildSwagger(this IServiceCollection service, IConfiguration configuration)
    {
        service
            .AddOpenApi()
            .AddEndpointsApiExplorer();
        SwaggerGen(service, configuration);
    }

    public static void UseCustomSwagger(this WebApplication app, IConfiguration configuration)
    {
        var swaggerSettings = Utils.Settings.GetSettings<SwaggerSetting>("Swagger", configuration);

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapSwagger();
            app
                .UseSwagger(options => { options.RouteTemplate = $"swagger/{{documentName}}/swagger.json"; })
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = swaggerSettings.Prefix;
                    options.SwaggerEndpoint($"/swagger/{swaggerSettings.Version}/swagger.json", swaggerSettings.Title);
                });
        }
    }
}
