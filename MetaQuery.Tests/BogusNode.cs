namespace MetaQuery.Tests;

public sealed record BogusNode : IMetaQueryNode
{
    public string Lol { get; init; } = "LOL";
}