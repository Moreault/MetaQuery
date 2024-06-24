namespace ToolBX.MetaQuery.Json;

public sealed class MetaQueryConditionConverter : JsonConverter<MetaQueryCondition>
{
    public override MetaQueryCondition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        string? field = null;
        ComparisonOperator? op = null;
        object? value = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new MetaQueryCondition
                {
                    Field = field ?? throw new JsonException("Field is required"),
                    Operator = op ?? throw new JsonException("Operator is required"),
                    Value = value 
                };
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName token");
            }

            var propertyName = reader.GetString()!;
            reader.Read();

            switch (propertyName)
            {
                case "Field":
                    field = reader.GetString();
                    break;
                case "Operator":
                    op = JsonSerializer.Deserialize<ComparisonOperator>(ref reader, options);
                    break;
                case "Value":
                    value = reader.TokenType switch
                    {
                        JsonTokenType.True => true,
                        JsonTokenType.False => false,
                        JsonTokenType.Number when reader.TryGetInt64(out var l) => l,
                        JsonTokenType.Number => reader.GetDouble(),
                        JsonTokenType.String when reader.TryGetDateTime(out var datetime) => datetime,
                        JsonTokenType.String => reader.GetString(),
                        // Add other types as needed (e.g., JsonTokenType.StartObject, JsonTokenType.StartArray)
                        _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
                    };
                    break;
                default:
                    throw new JsonException($"Unknown property: {propertyName}");
            }
        }

        throw new JsonException("Expected EndObject token");
    }

    public override void Write(Utf8JsonWriter writer, MetaQueryCondition value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Field", value.Field);
        writer.WriteString("Operator", value.Operator.ToString());
        writer.WritePropertyName("Value");
        JsonSerializer.Serialize(writer, value.Value, options);
        writer.WriteEndObject();
    }
}
