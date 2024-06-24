namespace MetaQuery.Tests;

[TestClass]
public class EnumerableParserTests
{
    [TestMethod]
    public void Parse_WhenPredicateIsNull_Throw()
    {
        //Arrange
        MetaQueryPredicate predicate = null!;

        //Act
        var action = () => EnumerableParser.Parse<Person>(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
    }

    //Other behaviors are tested via EnumerableExtensionsTester
}