namespace ToolBX.MetaQuery.Json;

public sealed class MetaQueryPredicateConverter : JsonConverter<MetaQueryPredicate>
{
    public override MetaQueryPredicate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        List<IMetaQueryNode>? nodes = null;
        LogicalOperator? logicalOperator = null!;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();

            reader.Read();

            switch (propertyName)
            {
                case "nodes":
                    nodes = JsonSerializer.Deserialize<List<IMetaQueryNode>>(ref reader, options);
                    break;
                case "operator":
                    logicalOperator = JsonSerializer.Deserialize<LogicalOperator>(ref reader, options);
                    break;
                default:
                    throw new JsonException();
            }
        }

        if (nodes is null || logicalOperator is null)
        {
            throw new JsonException("Invalid JSON for MetaQueryPredicate");
        }

        return new MetaQueryPredicate(nodes) { Operator = logicalOperator.Value };
    }

    public override void Write(Utf8JsonWriter writer, MetaQueryPredicate value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("nodes");
        JsonSerializer.Serialize(writer, value.ToList(), options);

        writer.WritePropertyName("operator");
        JsonSerializer.Serialize(writer, value.Operator, options);

        writer.WriteEndObject();
    }
}
