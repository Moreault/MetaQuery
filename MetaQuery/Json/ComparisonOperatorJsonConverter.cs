namespace ToolBX.MetaQuery.Json;

public class ComparisonOperatorJsonConverter : JsonConverter<ComparisonOperator>
{
    public override ComparisonOperator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string value.");
        }

        var stringValue = reader.GetString()!;
        foreach (var field in typeof(ComparisonOperator).GetFields())
        {
            if (field.Name == stringValue)
                return (ComparisonOperator)field.GetValue(null)!;

            var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute?.Value == stringValue)
                return (ComparisonOperator)field.GetValue(null)!;
        }

        throw new JsonException($"Unable to convert \"{stringValue}\" to ComparisonOperator.");
    }

    public override void Write(Utf8JsonWriter writer, ComparisonOperator value, JsonSerializerOptions options)
    {
        var field = typeof(ComparisonOperator).GetField(value.ToString());
        var descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();
        var stringValue = descriptionAttribute?.Value ?? value.ToString();
        writer.WriteStringValue(stringValue);
    }
}