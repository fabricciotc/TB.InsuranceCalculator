using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TB.Insurance.Api.OpenApi;

internal sealed class EnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        if (!type.IsEnum)
            return Task.CompletedTask;

        var values = Enum.GetValues(type)
            .Cast<object>()
            .Select(value => (Name: Enum.GetName(type, value)!, Number: Convert.ToInt32(value)))
            .ToArray();

        schema.Type = JsonSchemaType.String;
        schema.Format = null;
        schema.Pattern = null;
        schema.Enum = values
            .Select(value => (JsonNode)JsonValue.Create(value.Name)!)
            .ToList();
        schema.Description = string.Join(" | ", values.Select(value => $"{value.Number} = {value.Name}"));

        return Task.CompletedTask;
    }
}
