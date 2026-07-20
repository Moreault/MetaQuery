namespace MetaQuery.Tests;

[TestClass]
public sealed class MetaQueryEvaluatorTests
{
    private static readonly MetaQuerySchema<Completion> Schema = CompletionSchema.Create();

    private static bool Evaluate(Completion item, string field, ComparisonOperator op, object? value)
        => MetaQueryEvaluator.Evaluate(new MetaQueryCondition { Field = field, Operator = op, Value = value }, item, Schema);

    [TestMethod]
    public void Equals_OnIntField_CoercesAndMatches()
    {
        var item = new Completion { MissionId = 5 };
        Assert.IsTrue(Evaluate(item, "MissionId", ComparisonOperator.Equals, 5));
        Assert.IsTrue(Evaluate(item, "MissionId", ComparisonOperator.Equals, 5L)); // JSON often yields long
        Assert.IsFalse(Evaluate(item, "MissionId", ComparisonOperator.Equals, 6));
    }

    [TestMethod]
    public void NotEquals_OnBoolField_Works()
    {
        var item = new Completion { IsCompleted = true };
        Assert.IsTrue(Evaluate(item, "IsCompleted", ComparisonOperator.Equals, true));
        Assert.IsFalse(Evaluate(item, "IsCompleted", ComparisonOperator.NotEquals, true));
    }

    [TestMethod]
    public void LessThan_OnTimeSpanField_ParsesStringValue()
    {
        var item = new Completion { BestTime = TimeSpan.FromMinutes(5) };
        Assert.IsTrue(Evaluate(item, "BestTime", ComparisonOperator.LessThan, "00:10:00"));
        Assert.IsFalse(Evaluate(item, "BestTime", ComparisonOperator.GreaterThan, "00:10:00"));
    }

    [TestMethod]
    public void Ordered_WhenLeftIsNull_ReturnsFalse()
    {
        var item = new Completion { BestTime = null };
        Assert.IsFalse(Evaluate(item, "BestTime", ComparisonOperator.LessThan, "00:10:00"));
        Assert.IsFalse(Evaluate(item, "BestTime", ComparisonOperator.GreaterThan, "00:10:00"));
    }

    [TestMethod]
    public void Contains_OnCollectionField_MatchesScalarValue()
    {
        var item = new Completion { CompletedObjectives = [1, 2, 3] };
        Assert.IsTrue(Evaluate(item, "CompletedObjectives", ComparisonOperator.Contains, 3));
        Assert.IsTrue(Evaluate(item, "CompletedObjectives", ComparisonOperator.Contains, 3L));
        Assert.IsFalse(Evaluate(item, "CompletedObjectives", ComparisonOperator.Contains, 99));
    }

    [TestMethod]
    public void IsIn_OnScalarField_MatchesAgainstCollectionValue()
    {
        var item = new Completion { MissionId = 2 };
        Assert.IsTrue(Evaluate(item, "MissionId", ComparisonOperator.IsIn, new List<object?> { 1, 2, 3 }));
        Assert.IsTrue(Evaluate(item, "MissionId", ComparisonOperator.IsNotIn, new List<object?> { 4, 5 }));
    }

    [TestMethod]
    public void Evaluate_WhenFieldIsNotRegistered_Throws()
    {
        var item = new Completion();
        Assert.ThrowsExactly<ArgumentException>(() => Evaluate(item, "Godliness", ComparisonOperator.Equals, 1));
    }
}
