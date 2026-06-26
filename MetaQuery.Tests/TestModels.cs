namespace MetaQuery.Tests;

/// <summary>Mirrors the shape of the game's mission-completion record (int / bool / TimeSpan? / collection).</summary>
public sealed record Completion
{
    public int MissionId { get; init; }
    public bool IsCompleted { get; init; }
    public TimeSpan? BestTime { get; init; }
    public IReadOnlyList<int> CompletedObjectives { get; init; } = [];
}

public static class CompletionSchema
{
    public static MetaQuerySchema<Completion> Create() => new MetaQuerySchema<Completion>()
        .Field("MissionId", x => x.MissionId)
        .Field("IsCompleted", x => x.IsCompleted)
        .Field("BestTime", x => x.BestTime)
        .Field("CompletedObjectives", x => x.CompletedObjectives);
}
