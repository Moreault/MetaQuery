namespace MetaQuery.Tests;

public record BogusNode : IMetaQueryNode
{
    public string Lol { get; init; } = "LOL";
}