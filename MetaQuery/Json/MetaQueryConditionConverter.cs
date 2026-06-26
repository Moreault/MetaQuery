using System.Globalization;

namespace ToolBX.MetaQuery.Json;

/// <summary>
/// AOT-safe converter : reads/writes <see cref="MetaQueryCondition.Value"/> through an explicit,
/// closed set of JSON-native scalar types (and arrays thereof) rather than reflecting over
/// <see cref="object"/>. Store values as JSON-native scalars (bool / number / string) so the
/// condition round-trips to an equal instance ; the evaluator coerces to the field's CLR type.
/// </summary>
public sealed class MetaQueryConditionConverter : JsonConverter<MetaQueryCondition>
{
    public override MetaQueryCondition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected StartObject token");

        string? field = null;
        ComparisonOperator? op = null;
        object? value = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new MetaQueryCondition
                {
                    Field = field ?? throw new JsonException("Field is required"),
                    Operator = op ?? throw new JsonException("Operator is required"),
                    Value = value
                };

            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException("Expected PropertyName token");

            var propertyName = reader.GetString()!;
            reader.Read();

            switch (propertyName)
            {
                case "Field":
                    field = reader.GetString();
                    break;
                case "Operator":
                    op = Enum.Parse<ComparisonOperator>(reader.GetString()!, ignoreCase: true);
                    break;
                case "Value":
                    value = ReadValue(ref reader);
                    break;
                default:
                    throw new JsonException($"Unknown property: {propertyName}");
            }
        }

        throw new JsonException("Expected EndObject token");
    }

    private static object? ReadValue(ref Utf8JsonReader reader)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.Number:
                if (reader.TryGetInt32(out var i)) return i;
                if (reader.TryGetInt64(out var l)) return l;
                return reader.GetDouble();
            case JsonTokenType.String:
                return reader.GetString();
            case JsonTokenType.StartArray:
                var list = new List<object?>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    list.Add(ReadValue(ref reader));
                return list;
            default:
                throw new JsonException($"Unsupported value token: {reader.TokenType}");
        }
    }

    public override void Write(Utf8JsonWriter writer, MetaQueryCondition value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Field", value.Field);
        writer.WriteString("Operator", value.Operator.ToString());
        writer.WritePropertyName("Value");
        WriteValue(writer, value.Value);
        writer.WriteEndObject();
    }

    private static void WriteValue(Utf8JsonWriter writer, object? value)
    {
        switch (value)
        {
            case null:
                writer.WriteNullValue();
                break;
            case bool b:
                writer.WriteBooleanValue(b);
                break;
            case int i:
                writer.WriteNumberValue(i);
                break;
            case long l:
                writer.WriteNumberValue(l);
                break;
            case double d:
                writer.WriteNumberValue(d);
                break;
            case float f:
                writer.WriteNumberValue(f);
                break;
            case string s:
                writer.WriteStringValue(s);
                break;
            case TimeSpan ts:
                writer.WriteStringValue(ts.ToString("c", CultureInfo.InvariantCulture));
                break;
            case IEnumerable enumerable:
                writer.WriteStartArray();
                foreach (var element in enumerable) WriteValue(writer, element);
                writer.WriteEndArray();
                break;
            default:
                throw new JsonException($"Unsupported value type for {nameof(MetaQueryCondition)}.{nameof(MetaQueryCondition.Value)}: {value.GetType()}");
        }
    }
}
