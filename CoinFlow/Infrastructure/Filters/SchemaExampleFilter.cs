namespace CoinFlow.Infrastructure.Filters;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Models.UserEntity.Dto;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SchemaExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(UserRequest))
        {
            schema.Properties["userName"].Example = new OpenApiString("johndoe");
            schema.Properties["name"].Example = new OpenApiString("John Doe");
            schema.Properties["email"].Example = new OpenApiString("john.doe@example.com");
            schema.Properties["password"].Example = new OpenApiString("SenhaSegura123@");
            schema.Properties["phoneNumber"].Example = new OpenApiString("+5511999999999");
        }
    }
}
