namespace ToolBX.MetaQuery;

public sealed record MetaQueryPredicate : IMetaQueryNode, IReadOnlyList<IMetaQueryNode>
{
    public IMetaQueryNode this[int index] => _nodes[index];

    private readonly IReadOnlyList<IMetaQueryNode> _nodes = null!;

    public int Count => _nodes.Count;

    public required LogicalOperator Operator
    {
        get => _operator;
        init => _operator = value.ThrowIfUndefined();
    }
    private readonly LogicalOperator _operator;

    public MetaQueryPredicate(params IMetaQueryNode[] nodes) : this(nodes as IEnumerable<IMetaQueryNode>)
    {

    }

    public MetaQueryPredicate(IEnumerable<IMetaQueryNode> nodes)
    {
        _nodes = nodes?.ToReadOnlyList() ?? throw new ArgumentNullException(nameof(nodes));
    }

    public bool Equals(MetaQueryPredicate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _nodes.SequenceEqual(other._nodes) && Operator.Equals(other.Operator);
    }

    public IEnumerator<IMetaQueryNode> GetEnumerator() => _nodes.GetEnumerator();

    public override int GetHashCode() => HashCode.Combine(_nodes, (int)Operator);

    public override string ToString() => _nodes.Any() ? $"({string.Join($" {Operator.GetDescription()} ", _nodes)})" : "*";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}