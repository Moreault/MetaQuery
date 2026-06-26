namespace MetaQuery.Tests;

[TestClass]
public sealed class SerializationTests
{
    private static T RoundTrip<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        return JsonSerializer.Deserialize<T>(json)!;
    }

    [TestMethod]
    public void Condition_WithIntValue_RoundTrips()
    {
        var condition = new MetaQueryCondition { Field = "MissionId", Operator = ComparisonOperator.Equals, Value = 5 };
        Assert.AreEqual(condition, RoundTrip(condition));
    }

    [TestMethod]
    public void Condition_WithBoolValue_RoundTrips()
    {
        var condition = new MetaQueryCondition { Field = "IsCompleted", Operator = ComparisonOperator.NotEquals, Value = true };
        Assert.AreEqual(condition, RoundTrip(condition));
    }

    [TestMethod]
    public void Condition_WithStringValue_RoundTrips()
    {
        var condition = new MetaQueryCondition { Field = "BestTime", Operator = ComparisonOperator.LessThan, Value = "00:10:00" };
        Assert.AreEqual(condition, RoundTrip(condition));
    }

    [TestMethod]
    public void Condition_WithNullValue_RoundTrips()
    {
        var condition = new MetaQueryCondition { Field = "BestTime", Operator = ComparisonOperator.NotEquals, Value = null };
        Assert.AreEqual(condition, RoundTrip(condition));
    }

    [TestMethod]
    public void Filter_WithMixedTerms_RoundTrips()
    {
        var filter = new MetaQueryFilter([
            new MetaQueryFilterTerm { Condition = new MetaQueryCondition { Field = "MissionId", Operator = ComparisonOperator.Equals, Value = 1 }, OperatorToNext = LogicalOperator.And },
            new MetaQueryFilterTerm { Condition = new MetaQueryCondition { Field = "BestTime", Operator = ComparisonOperator.LessThan, Value = "00:10:00" }, OperatorToNext = LogicalOperator.Or },
            new MetaQueryFilterTerm { Condition = new MetaQueryCondition { Field = "CompletedObjectives", Operator = ComparisonOperator.Contains, Value = 3 }, OperatorToNext = LogicalOperator.And }
        ]);

        Assert.AreEqual(filter, RoundTrip(filter));
    }
}
