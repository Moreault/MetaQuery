using ToolBX.Collections.ReadOnly;

namespace MetaQuery.Tests;

public static class Database
{
    public static IReadOnlyList<Person> People = new List<Person>
    {
        new()
        {
            Id = 1, Name = "Carol", Age = 29, Job = new Job("Bounty hunter", 31000), Hobbies = new ReadOnlyList<Hobby>(new Hobby("Knitting"))
        },
        new()
        {
            Id = 2, Name = "Roger", Age = 15, Job = new Job("Paper boy", 200), Hobbies = new ReadOnlyList<Hobby>(new Hobby("Crossplay"), new Hobby("Video games"))
        },
        new()
        {
            Id = 3, Name = "Nancy", Age = 8, Hobbies = new ReadOnlyList<Hobby>(new Hobby("Picking nose"), new Hobby("Crying wolf"))
        },
        new()
        {
            Id = 4, Name = "Seamus", Age = 56, Job = new Job("Cashier", 89232), Hobbies = new ReadOnlyList<Hobby>(new Hobby("Yelling at clouds"), new Hobby("Complaining about young people"))
        },
        new()
        {
            Id = 5, Name = "Agatha", Age = 83, Job = new Job("Retired", 212000), Hobbies = new ReadOnlyList<Hobby>(new Hobby("Talking to random strangers"), new Hobby("Feeding pigeons"))
        },
        new()
        {
            Id = 6, Name = "Seb", Age = 35, Job = new Job("Engineer", 2419250), Hobbies = new ReadOnlyList<Hobby>(new Hobby("Creating life"), new Hobby("Building Tokyo Towers"), new Hobby("Farting on squirrels"))
        }
    };
}