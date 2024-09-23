using Newtonsoft.Json;
using ToolBX.MetaQuery.Json;

namespace MetaQuery.Tests;

[TestClass]
public class MetaQueryPredicateTests : RecordTester<MetaQueryPredicate>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        JsonSerializerOptions.Converters.Add(new MetaQueryConditionConverter());
        JsonSerializerOptions.Converters.Add(new MetaQueryPredicateConverter());

        //var anus = JsonConvert.SerializeObject(Dummy.Create<MetaQueryPredicate>());


    }

    [TestMethod]
    public void METHOD_WhenWHEN_THEN()
    {
        //Arrange

        //Act
        Dummy.Create<MetaQueryPredicate>();

        //Assert
    }

    [TestMethod]
    public void Nodes_WhenIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<IMetaQueryNode> nodes = null!;

        //Act
        var action = () => new MetaQueryPredicate(nodes) { Operator = Dummy.Create<LogicalOperator>() };

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(nodes));
    }

    [TestMethod]
    public void Nodes_WhenIsNotNull_SetValue()
    {
        //Arrange
        var nodes = Dummy.CreateMany<IMetaQueryNode>().ToList();

        //Act
        var result = new MetaQueryPredicate(nodes) { Operator = Dummy.Create<LogicalOperator>() };

        //Assert
        result.Should().BeEquivalentTo(nodes);
    }

    [TestMethod]
    public void Nodes_WhenIsNotNull_ModifyingOriginalCollectionDoesNotModifyNodes()
    {
        //Arrange
        var nodes = Dummy.CreateMany<IMetaQueryNode>().ToList();
        var copy = nodes.ToList();

        //Act
        var result = new MetaQueryPredicate(nodes) { Operator = Dummy.Create<LogicalOperator>() };

        //Assert
        nodes.RemoveAt(1);
        result.Should().BeEquivalentTo(copy);
        result.Should().NotBeEquivalentTo(nodes);
    }

    [TestMethod]
    public void Operator_WhenIsUndefined_Throw()
    {
        //Arrange
        var operatorValue = (LogicalOperator)(-Dummy.Create<int>());

        //Act
        var action = () => new MetaQueryPredicate(Dummy.CreateMany<IMetaQueryNode>()) { Operator = operatorValue };

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Operator_WhenIsDefined_SetValue()
    {
        //Arrange
        var operatorValue = Dummy.Create<LogicalOperator>();

        //Act
        var result = new MetaQueryPredicate(Dummy.CreateMany<IMetaQueryNode>()) { Operator = operatorValue };

        //Assert
        result.Operator.Should().Be(operatorValue);
    }

    [TestMethod]
    public void ToString_WhenNodesAreEmpty_ReturnAsterix()
    {
        //Arrange
        var instance = new MetaQueryPredicate { Operator = Dummy.Create<LogicalOperator>() };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("*");
    }

    [TestMethod]
    public void ToString_WhenNodesAreNotEmpty_ReturnBetweenParenthesises()
    {
        //Arrange
        var instance = new MetaQueryPredicate(new MetaQueryCondition { Field = "Name", Operator = ComparisonOperator.NotEquals, Value = "Seb" },
            new MetaQueryCondition { Field = "Age", Operator = ComparisonOperator.GreaterThan, Value = 5 })
        {
            Operator = LogicalOperator.And
        };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("(Name != Seb && Age > 5)");
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<MetaQueryPredicate>(Dummy, JsonSerializerOptions);
}