namespace MetaQuery.Tests;

[TestClass]
public sealed class ComparisonOperatorExtensionsTests : Tester
{
    [TestMethod]
    public void ToBinaryExpression_WhenLeftIsNull_Throw()
    {
        //Arrange
        var value = Dummy.Create<ComparisonOperator>();
        MemberExpression left = null!;
        var right = Expression.Constant(Dummy.Create<string>());

        //Act
        var action = () => value.ToBinaryExpression(left, right);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(left));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenRightIsNull_Throw()
    {
        //Arrange
        var value = Dummy.Create<ComparisonOperator>();
        var left = Expression.Parameter(typeof(Person));
        ConstantExpression right = null!;

        //Act
        var action = () => value.ToBinaryExpression(left, right);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(right));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsUnrecognized_Throw()
    {
        //Arrange
        var value = (ComparisonOperator)(-Dummy.Create<int>());
        var left = Expression.Parameter(typeof(Person));
        var right = Expression.Constant(Dummy.Create<string>());

        //Act
        var action = () => value.ToBinaryExpression(left, right);

        //Assert
        action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.ComparisonOperatorUnsupported, value));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsEqual_ReturnEqual()
    {
        //Arrange
        var value = ComparisonOperator.Equals;
        var left = Expression.Parameter(typeof(string));
        var right = Expression.Constant(Dummy.Create<string>());

        //Act
        var result = value.ToBinaryExpression(left, right);

        //Assert
        result.Should().BeEquivalentTo(Expression.Equal(left, right));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsNotEquals_ReturnNotEqual()
    {
        //Arrange
        var value = ComparisonOperator.NotEquals;
        var left = Expression.Parameter(typeof(string));
        var right = Expression.Constant(Dummy.Create<string>());

        //Act
        var result = value.ToBinaryExpression(left, right);

        //Assert
        result.Should().BeEquivalentTo(Expression.NotEqual(left, right));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsGreaterThan_ReturnGreaterThan()
    {
        //Arrange
        var value = ComparisonOperator.GreaterThan;
        var left = Expression.Parameter(typeof(int));
        var right = Expression.Constant(Dummy.Create<int>());

        //Act
        var result = value.ToBinaryExpression(left, right);

        //Assert
        result.Should().BeEquivalentTo(Expression.GreaterThan(left, right));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsGreaterThanOrEquals_ReturnGreaterThanOrEqual()
    {
        //Arrange
        var value = ComparisonOperator.GreaterThanOrEquals;
        var left = Expression.Parameter(typeof(int));
        var right = Expression.Constant(Dummy.Create<int>());

        //Act
        var result = value.ToBinaryExpression(left, right);

        //Assert
        result.Should().BeEquivalentTo(Expression.GreaterThanOrEqual(left, right));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsLessThan_ReturnLessThan()
    {
        //Arrange
        var value = ComparisonOperator.LessThan;
        var left = Expression.Parameter(typeof(int));
        var right = Expression.Constant(Dummy.Create<int>());

        //Act
        var result = value.ToBinaryExpression(left, right);

        //Assert
        result.Should().BeEquivalentTo(Expression.LessThan(left, right));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsLessThanOrEquals_ReturnLessThanOrEqual()
    {
        //Arrange
        var value = ComparisonOperator.LessThanOrEquals;
        var left = Expression.Parameter(typeof(int));
        var right = Expression.Constant(Dummy.Create<int>());

        //Act
        var result = value.ToBinaryExpression(left, right);

        //Assert
        result.Should().BeEquivalentTo(Expression.LessThanOrEqual(left, right));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsIn_Throw()
    {
        //Arrange
        var value = ComparisonOperator.IsIn;
        var left = Expression.Parameter(typeof(string));
        var right = Expression.Constant(Dummy.Create<string>());

        //Act
        var action = () => value.ToBinaryExpression(left, right);

        //Assert
        action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.ComparisonOperatorUnsupported, value));
    }

    [TestMethod]
    public void ToBinaryExpression_WhenValueIsNotIn_Throw()
    {
        //Arrange
        var value = ComparisonOperator.IsNotIn;
        var left = Expression.Parameter(typeof(string));
        var right = Expression.Constant(Dummy.Create<string>());

        //Act
        var action = () => value.ToBinaryExpression(left, right);

        //Assert
        action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.ComparisonOperatorUnsupported, value));
    }
}