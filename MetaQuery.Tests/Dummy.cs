namespace MetaQuery.Tests;

public record Person
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public Job Job { get; init; } = new("Jobless", 0);
    public IReadOnlyList<Hobby> Hobbies { get; init; } = Array.Empty<Hobby>();
}

public record Job(string Title, float Salary);

public record Hobby(string Name);