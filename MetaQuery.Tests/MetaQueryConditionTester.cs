using ToolBX.DescriptiveEnums;

namespace MetaQuery.Tests;

[TestClass]
public class MetaQueryConditionTester
{
    [TestClass]
    public class Field : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenIsNullOrEmpty_Throw(string value)
        {
            //Arrange

            //Act
            var action = () => new MetaQueryCondition { Field = value, Operator = Fixture.Create<ComparisonOperator>(), Value = Fixture.Create<object>() };

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(value));
        }

        [TestMethod]
        public void WhenIsNotNull_SetValue()
        {
            //Arrange
            var value = Fixture.Create<string>();

            //Act
            var result = new MetaQueryCondition { Field = value, Operator = Fixture.Create<ComparisonOperator>(), Value = Fixture.Create<object>() };

            //Assert
            result.Field.Should().Be(value);
        }
    }

    [TestClass]
    public class Operator : Tester
    {
        [TestMethod]
        public void WhenIsValueUndefined_Throw()
        {
            //Arrange
            var value = (ComparisonOperator)(-Fixture.Create<int>());

            //Act
            var action = () => new MetaQueryCondition { Field = Fixture.Create<string>(), Operator = value, Value = Fixture.Create<object>() };

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenValueIsDefined_SetValue()
        {
            //Arrange
            var value = Fixture.Create<ComparisonOperator>();

            //Act
            var result = new MetaQueryCondition { Field = Fixture.Create<string>(), Operator = value, Value = Fixture.Create<object>() };

            //Assert
            result.Operator.Should().Be(value);
        }
    }

    [TestClass]
    public class Value : Tester
    {
        [TestMethod]
        public void Always_SetValue()
        {
            //Arrange
            var value = Fixture.Create<object>();

            //Act
            var result = new MetaQueryCondition { Field = Fixture.Create<string>(), Operator = Fixture.Create<ComparisonOperator>(), Value = value };

            //Assert
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void WhenIsNull_SetNullValue()
        {
            //Arrange

            //Act
            var result = new MetaQueryCondition { Field = Fixture.Create<string>(), Operator = Fixture.Create<ComparisonOperator>(), Value = null };

            //Assert
            result.Value.Should().BeNull();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester
    {
        [TestMethod]
        public void Always_OutputFieldOperatorAndValue()
        {
            //Arrange
            var condition = Fixture.Create<MetaQueryCondition>();

            //Act
            var result = condition.ToString();

            //Assert
            result.Should().Be($"{condition.Field} {condition.Operator.GetDescription()} {condition.Value}");
        }

        [TestMethod]
        public void WhenValueIsNull_ShowNull()
        {
            //Arrange
            var condition = Fixture.Create<MetaQueryCondition>() with { Value = null };

            //Act
            var result = condition.ToString();

            //Assert
            result.Should().Be($"{condition.Field} {condition.Operator.GetDescription()} NULL");
        }

        [TestMethod]
        public void WhenValueIsAnEmptyString_ShowEmptyQuotes()
        {
            //Arrange
            var condition = Fixture.Build<MetaQueryCondition>().With(x => x.Value, string.Empty).Create();

            //Act
            var result = condition.ToString();

            //Assert
            result.Should().Be($@"{condition.Field} {condition.Operator.GetDescription()} """);
        }
    }
}
