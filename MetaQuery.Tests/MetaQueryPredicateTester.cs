namespace MetaQuery.Tests;

[TestClass]
public class MetaQueryPredicateTester
{
    [TestClass]
    public class Nodes : Tester
    {
        [TestMethod]
        public void WhenIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<IMetaQueryNode> nodes = null!;

            //Act
            var action = () => new MetaQueryPredicate(nodes) { Operator = Fixture.Create<LogicalOperator>() };

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(nodes));
        }

        [TestMethod]
        public void WhenIsNotNull_SetValue()
        {
            //Arrange
            var nodes = Fixture.CreateMany<IMetaQueryNode>().ToList();

            //Act
            var result = new MetaQueryPredicate(nodes) { Operator = Fixture.Create<LogicalOperator>() };

            //Assert
            result.Should().BeEquivalentTo(nodes);
        }

        [TestMethod]
        public void WhenIsNotNull_ModifyingOriginalCollectionDoesNotModifyNodes()
        {
            //Arrange
            var nodes = Fixture.CreateMany<IMetaQueryNode>().ToList();
            var copy = nodes.ToList();

            //Act
            var result = new MetaQueryPredicate(nodes) { Operator = Fixture.Create<LogicalOperator>() };

            //Assert
            nodes.RemoveAt(1);
            result.Should().BeEquivalentTo(copy);
            result.Should().NotBeEquivalentTo(nodes);
        }
    }

    [TestClass]
    public class Operator : Tester
    {
        [TestMethod]
        public void WhenIsUndefined_Throw()
        {
            //Arrange
            var operatorValue = (LogicalOperator)(-Fixture.Create<int>());

            //Act
            var action = () => new MetaQueryPredicate(Fixture.CreateMany<IMetaQueryNode>()) { Operator = operatorValue };

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenIsDefined_SetValue()
        {
            //Arrange
            var operatorValue = Fixture.Create<LogicalOperator>();

            //Act
            var result = new MetaQueryPredicate(Fixture.CreateMany<IMetaQueryNode>()) { Operator = operatorValue };

            //Assert
            result.Operator.Should().Be(operatorValue);
        }
    }

    [TestClass]
    public class EqualsMethod : Tester
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<MetaQueryPredicate>();

            //Act
            var result = instance.Equals(null);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            var instance = Fixture.Create<MetaQueryPredicate>();

            //Act
            var result = instance.Equals(instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsDifferentObjectReferenceButEquivalent_ReturnTrue()
        {
            //Arrange
            var instance = Fixture.Create<MetaQueryPredicate>();
            var other = instance with { };

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsDifferentObjectReferenceAndNotEquivalent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<MetaQueryPredicate>();
            var other = Fixture.Create<MetaQueryPredicate>();

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester
    {
        [TestMethod]
        public void WhenNodesAreEmpty_ReturnAsterix()
        {
            //Arrange
            var instance = new MetaQueryPredicate { Operator = Fixture.Create<LogicalOperator>() };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("*");
        }

        [TestMethod]
        public void WhenNodesAreNotEmpty_ReturnBetweenParenthesises()
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
    }
}