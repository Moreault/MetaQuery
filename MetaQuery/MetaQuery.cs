namespace ToolBX.MetaQuery;

public record MetaQuery
{
    public required MetaQueryPredicate Predicate
    {
        get => _predicate;
        init => _predicate = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly MetaQueryPredicate _predicate = null!;

    public override string ToString()
    {
        return $"Where {Predicate}";
    }
}