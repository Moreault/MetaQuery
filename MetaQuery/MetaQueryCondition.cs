namespace ToolBX.MetaQuery;

[JsonConverter(typeof(MetaQueryConditionConverter))]
public sealed class MetaQueryCondition : IEquatable<MetaQueryCondition>
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

    public required object? Value { get; init; }

    public MetaQueryCondition Clone() => new()
    {
        Field = Field,
        Operator = Operator,
        Value = Value
    };

    public bool Equals(MetaQueryCondition? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(_field, other._field, StringComparison.Ordinal) &&
               _operator == other._operator &&
               Equals(Value, other.Value);
    }

    public override bool Equals(object? obj) => Equals(obj as MetaQueryCondition);

    public override int GetHashCode() => HashCode.Combine(_field, (int)_operator, Value);

    public static bool operator ==(MetaQueryCondition? a, MetaQueryCondition? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(MetaQueryCondition? a, MetaQueryCondition? b) => !(a == b);

    public override string ToString() => $"{Field} {Operator.GetDescription()} {Value ?? "NULL"}";
}
