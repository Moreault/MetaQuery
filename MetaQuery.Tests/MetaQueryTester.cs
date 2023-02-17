namespace MetaQuery.Tests;

[TestClass]
public class MetaQueryTester
{
    [TestClass]
    public class Predicate : Tester
    {
        [TestMethod]
        public void WhenValueIsNull_Throw()
        {
            //Arrange
            MetaQueryPredicate value = null!;

            //Act
            var action = () => new ToolBX.MetaQuery.MetaQuery { Predicate = value };

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(value));
        }

        [TestMethod]
        public void WhenValueIsNotNull_SetValue()
        {
            //Arrange
            var value = Fixture.Create<MetaQueryPredicate>();

            //Act
            var result = new ToolBX.MetaQuery.MetaQuery { Predicate = value };

            //Assert
            result.Predicate.Should().BeEquivalentTo(value);
        }
    }

    [TestClass]
    public class ToStringMethod : Tester
    {
        [TestMethod]
        public void WhenPredicateIsEmpty_ReturnWhereStar()
        {
            //Arrange
            var query = new ToolBX.MetaQuery.MetaQuery { Predicate = new MetaQueryPredicate { Operator = LogicalOperator.And } };

            //Act
            var result = query.ToString();

            //Assert
            result.Should().Be("Where *");
        }

        [TestMethod]
        public void WhenPredicateIsNotEmpty_ReturnFullWhereClause()
        {
            //Arrange
            var query = Fixture.Create<ToolBX.MetaQuery.MetaQuery>();

            //Act
            var result = query.ToString();

            //Assert
            result.Should().Be($"Where {query.Predicate}");
        }
    }
}