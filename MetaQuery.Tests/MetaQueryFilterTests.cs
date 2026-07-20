namespace MetaQuery.Tests;

[TestClass]
public sealed class MetaQueryFilterTests
{
    private static readonly MetaQuerySchema<Completion> Schema = CompletionSchema.Create();

    private static MetaQueryFilterTerm Term(string field, ComparisonOperator op, object? value, LogicalOperator next = LogicalOperator.And)
        => new() { Condition = new MetaQueryCondition { Field = field, Operator = op, Value = value }, OperatorToNext = next };

    [TestMethod]
    public void EmptyFilter_IsAlwaysSatisfied()
    {
        Assert.IsTrue(new MetaQueryFilter().IsSatisfiedBy(new Completion(), Schema));
        Assert.IsTrue(MetaQueryFilter.Empty.IsSatisfiedBy(new Completion(), Schema));
    }

    [TestMethod]
    public void SingleTerm_EvaluatesThatTerm()
    {
        var filter = new MetaQueryFilter([Term("IsCompleted", ComparisonOperator.Equals, true)]);
        Assert.IsTrue(filter.IsSatisfiedBy(new Completion { IsCompleted = true }, Schema));
        Assert.IsFalse(filter.IsSatisfiedBy(new Completion { IsCompleted = false }, Schema));
    }

    [TestMethod]
    public void And_RequiresBothTerms()
    {
        var filter = new MetaQueryFilter([
            Term("MissionId", ComparisonOperator.Equals, 5, LogicalOperator.And),
            Term("IsCompleted", ComparisonOperator.Equals, true)
        ]);
        Assert.IsTrue(filter.IsSatisfiedBy(new Completion { MissionId = 5, IsCompleted = true }, Schema));
        Assert.IsFalse(filter.IsSatisfiedBy(new Completion { MissionId = 5, IsCompleted = false }, Schema));
    }

    [TestMethod]
    public void Or_RequiresEitherTerm()
    {
        var filter = new MetaQueryFilter([
            Term("MissionId", ComparisonOperator.Equals, 5, LogicalOperator.Or),
            Term("MissionId", ComparisonOperator.Equals, 6)
        ]);
        Assert.IsTrue(filter.IsSatisfiedBy(new Completion { MissionId = 6 }, Schema));
        Assert.IsFalse(filter.IsSatisfiedBy(new Completion { MissionId = 7 }, Schema));
    }

    [TestMethod]
    public void Composition_IsLeftToRight_AAndBOrC()
    {
        // (MissionId == 1 AND IsCompleted == true) OR Contains 9
        var filter = new MetaQueryFilter([
            Term("MissionId", ComparisonOperator.Equals, 1, LogicalOperator.And),
            Term("IsCompleted", ComparisonOperator.Equals, true, LogicalOperator.Or),
            Term("CompletedObjectives", ComparisonOperator.Contains, 9)
        ]);

        Assert.IsTrue(filter.IsSatisfiedBy(new Completion { MissionId = 1, IsCompleted = true }, Schema));
        Assert.IsTrue(filter.IsSatisfiedBy(new Completion { MissionId = 0, CompletedObjectives = [9] }, Schema));
        Assert.IsFalse(filter.IsSatisfiedBy(new Completion { MissionId = 1, IsCompleted = false }, Schema));
    }

    [TestMethod]
    public void Clone_ProducesEqualButDistinctInstance()
    {
        var filter = new MetaQueryFilter([Term("MissionId", ComparisonOperator.Equals, 5)]);
        var clone = filter.Clone();

        Assert.AreEqual(filter, clone);
        Assert.AreNotSame(filter, clone);
        Assert.AreNotSame(filter.Terms[0], clone.Terms[0]);
    }
}
