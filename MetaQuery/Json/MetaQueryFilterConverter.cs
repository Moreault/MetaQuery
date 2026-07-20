namespace ToolBX.MetaQuery.Json;

public sealed class MetaQueryFilterConverter : JsonConverter<MetaQueryFilter>
{
    private static readonly MetaQueryConditionConverter ConditionConverter = new();

    public override MetaQueryFilter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // A filter serializes as a bare array of terms.
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException("Expected StartArray token");

        var terms = new List<MetaQueryFilterTerm>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray) break;
            terms.Add(ReadTerm(ref reader, options));
        }

        return new MetaQueryFilter(terms);
    }

    private static MetaQueryFilterTerm ReadTerm(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected StartObject token");

        MetaQueryCondition? condition = null;
        LogicalOperator op = default;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new MetaQueryFilterTerm
                {
                    Condition = condition ?? throw new JsonException("Condition is required"),
                    OperatorToNext = op
                };

            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException("Expected PropertyName token");

            var propertyName = reader.GetString()!;
            reader.Read();

            switch (propertyName)
            {
                case "Condition":
                    condition = ConditionConverter.Read(ref reader, typeof(MetaQueryCondition), options);
                    break;
                case "OperatorToNext":
                    op = Enum.Parse<LogicalOperator>(reader.GetString()!, ignoreCase: true);
                    break;
                default:
                    throw new JsonException($"Unknown property: {propertyName}");
            }
        }

        throw new JsonException("Expected EndObject token");
    }

    public override void Write(Utf8JsonWriter writer, MetaQueryFilter value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var term in value.Terms)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Condition");
            ConditionConverter.Write(writer, term.Condition, options);
            writer.WriteString("OperatorToNext", term.OperatorToNext.ToString());
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }
}
