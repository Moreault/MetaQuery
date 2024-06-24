using ToolBX.Reflection4Humans.ValueEquality;

namespace ToolBX.MetaQuery;

[JsonConverter(typeof(MetaQueryConditionConverter))]
public sealed record MetaQueryCondition : IMetaQueryNode
{
    public required string Field
    {
        get => _field;
        init
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            _field = value;
        }
    }
    private readonly string _field = null!;

    public required ComparisonOperator Operator
    {
        get => _operator;
        init => _operator = value.ThrowIfUndefined();
    }
    private readonly ComparisonOperator _operator;

    public required object? Value
    {
        get => _value;
        init
        {
            _value = value;
            if (value is null) _valueAsString = "NULL";
            else
            {
                var valueString = value.ToString()!;
                if (string.IsNullOrEmpty(valueString))
                    valueString = @"""";
                _valueAsString = valueString;
            }
        }
    }
    private readonly object? _value;

    private readonly string _valueAsString = null!;

    public bool Equals(MetaQueryCondition? other) => this.ValueEquals(other);

    public override int GetHashCode() => this.GetValueHashCode();

    public override string ToString() => $"{Field} {Operator.GetDescription()} {_valueAsString}";
}