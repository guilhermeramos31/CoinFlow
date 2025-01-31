namespace CoinFlow.Infrastructure.Filters;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Models.UserEntity.Dto;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SchemaExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var propertyExamples = new Dictionary<string, IOpenApiAny>
        {
            { "userName", new OpenApiString("johndoe") },
            { "name", new OpenApiString("John Doe") },
            { "email", new OpenApiString("john.doe@example.com") },
            { "password", new OpenApiString("SenhaSegura123@") },
            { "phoneNumber", new OpenApiString("+5511999999999") },
        };

        foreach (var property in schema.Properties)
        {
            if (propertyExamples.TryGetValue(property.Key, out var example))
            {
                property.Value.Example = example;
            }
        }
    }
}
