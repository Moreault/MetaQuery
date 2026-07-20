namespace ToolBX.MetaQuery;

/// <summary>
/// A flat, ordered list of <see cref="MetaQueryFilterTerm"/> evaluated strictly left-to-right
/// (left-associative ; no operator precedence, no nesting). An empty filter is always satisfied.
/// </summary>
[JsonConverter(typeof(MetaQueryFilterConverter))]
public sealed class MetaQueryFilter : IEquatable<MetaQueryFilter>
{
    public static readonly MetaQueryFilter Empty = new();

    public IReadOnlyList<MetaQueryFilterTerm> Terms { get; }

    public MetaQueryFilter() : this([])
    {
    }

    public MetaQueryFilter(IEnumerable<MetaQueryFilterTerm> terms)
    {
        Terms = terms?.ToList() ?? throw new ArgumentNullException(nameof(terms));
    }

    public bool IsSatisfiedBy<T>(T item, IMetaQuerySchema<T> schema)
    {
        if (schema is null) throw new ArgumentNullException(nameof(schema));
        if (Terms.Count == 0) return true;

        var result = MetaQueryEvaluator.Evaluate(Terms[0].Condition, item, schema);
        for (var i = 1; i < Terms.Count; i++)
        {
            var next = MetaQueryEvaluator.Evaluate(Terms[i].Condition, item, schema);
            result = Terms[i - 1].OperatorToNext == LogicalOperator.Or ? result || next : result && next;
        }
        return result;
    }

    public MetaQueryFilter Clone() => new(Terms.Select(x => x.Clone()));

    public bool Equals(MetaQueryFilter? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Terms.SequenceEqual(other.Terms);
    }

    public override bool Equals(object? obj) => Equals(obj as MetaQueryFilter);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var term in Terms) hash.Add(term);
        return hash.ToHashCode();
    }

    public static bool operator ==(MetaQueryFilter? a, MetaQueryFilter? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(MetaQueryFilter? a, MetaQueryFilter? b) => !(a == b);

    public override string ToString() => Terms.Count == 0 ? "*" : string.Join(" ", Terms);
}
