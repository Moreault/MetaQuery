namespace ToolBX.MetaQuery;

/// <summary>
/// A single condition within a <see cref="MetaQueryFilter"/>, together with the logical operator that
/// joins it to the <em>next</em> term. The operator on the last term is ignored.
/// </summary>
public sealed class MetaQueryFilterTerm : IEquatable<MetaQueryFilterTerm>
{
    public required MetaQueryCondition Condition
    {
        get => _condition;
        init => _condition = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly MetaQueryCondition _condition = null!;

    public LogicalOperator OperatorToNext { get; init; }

    public MetaQueryFilterTerm Clone() => new()
    {
        Condition = _condition.Clone(),
        OperatorToNext = OperatorToNext
    };

    public bool Equals(MetaQueryFilterTerm? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _condition.Equals(other._condition) && OperatorToNext == other.OperatorToNext;
    }

    public override bool Equals(object? obj) => Equals(obj as MetaQueryFilterTerm);

    public override int GetHashCode() => HashCode.Combine(_condition, (int)OperatorToNext);

    public static bool operator ==(MetaQueryFilterTerm? a, MetaQueryFilterTerm? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(MetaQueryFilterTerm? a, MetaQueryFilterTerm? b) => !(a == b);

    public override string ToString() => $"{Condition} {OperatorToNext.GetDescription()}";
}
