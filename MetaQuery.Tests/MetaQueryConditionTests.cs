using System.Text.Json;
using ToolBX.MetaQuery.Json;
using ToolBX.Reflection4Humans.ValueEquality;

namespace MetaQuery.Tests;

[TestClass]
public class MetaQueryConditionTests : RecordTester<MetaQueryCondition>
{
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Field_WhenIsNullOrEmpty_Throw(string value)
    {
        //Arrange

        //Act
        var action = () => new MetaQueryCondition { Field = value, Operator = Dummy.Create<ComparisonOperator>(), Value = Dummy.Create<object>() };

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(value));
    }

    [TestMethod]
    public void Field_WhenIsNotNull_SetValue()
    {
        //Arrange
        var value = Dummy.Create<string>();

        //Act
        var result = new MetaQueryCondition { Field = value, Operator = Dummy.Create<ComparisonOperator>(), Value = Dummy.Create<object>() };

        //Assert
        result.Field.Should().Be(value);
    }

    [TestMethod]
    public void Operator_WhenIsValueUndefined_Throw()
    {
        //Arrange
        var value = (ComparisonOperator)(-Dummy.Create<int>());

        //Act
        var action = () => new MetaQueryCondition { Field = Dummy.Create<string>(), Operator = value, Value = Dummy.Create<object>() };

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Operator_WhenValueIsDefined_SetValue()
    {
        //Arrange
        var value = Dummy.Create<ComparisonOperator>();

        //Act
        var result = new MetaQueryCondition { Field = Dummy.Create<string>(), Operator = value, Value = Dummy.Create<object>() };

        //Assert
        result.Operator.Should().Be(value);
    }

    [TestMethod]
    public void Value_Always_SetValue()
    {
        //Arrange
        var value = Dummy.Create<object>();

        //Act
        var result = new MetaQueryCondition { Field = Dummy.Create<string>(), Operator = Dummy.Create<ComparisonOperator>(), Value = value };

        //Assert
        result.Value.Should().Be(value);
    }

    [TestMethod]
    public void Value_WhenIsNull_SetNullValue()
    {
        //Arrange

        //Act
        var result = new MetaQueryCondition { Field = Dummy.Create<string>(), Operator = Dummy.Create<ComparisonOperator>(), Value = null };

        //Assert
        result.Value.Should().BeNull();
    }

    [TestMethod]
    public void ToString_Always_OutputFieldOperatorAndValue()
    {
        //Arrange
        var condition = Dummy.Create<MetaQueryCondition>();

        //Act
        var result = condition.ToString();

        //Assert
        result.Should().Be($"{condition.Field} {condition.Operator.GetDescription()} {condition.Value}");
    }

    [TestMethod]
    public void ToString_WhenValueIsNull_ShowNull()
    {
        //Arrange
        var condition = Dummy.Create<MetaQueryCondition>() with { Value = null };

        //Act
        var result = condition.ToString();

        //Assert
        result.Should().Be($"{condition.Field} {condition.Operator.GetDescription()} NULL");
    }

    [TestMethod]
    public void ToString_WhenValueIsAnEmptyString_ShowEmptyQuotes()
    {
        //Arrange
        var condition = Dummy.Build<MetaQueryCondition>().With(x => x.Value, string.Empty).Create();

        //Act
        var result = condition.ToString();

        //Assert
        result.Should().Be($@"{condition.Field} {condition.Operator.GetDescription()} """);
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<MetaQueryCondition>(Dummy, JsonSerializerOptions);
}
