namespace MetaQuery.Tests
{
    [TestClass]
    public class EnumerableExtensionsTester
    {
        [TestClass]
        public class Where : Tester
        {
            [TestMethod]
            public void WhenSourceIsNull_Throw()
            {
                //Arrange
                IEnumerable<Person> source = null!;
                var predicate = Fixture.Create<MetaQueryPredicate>();

                //Act
                var action = () => source.Where(predicate);

                //Assert
                action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
            }

            [TestMethod]
            public void WhenPredicateIsNull_Throw()
            {
                //Arrange
                MetaQueryPredicate predicate = null!;

                //Act
                var action = () => Database.People.Where(predicate);

                //Assert
                action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
            }

            [TestMethod]
            public void WhenPredicateReferencesInexistantField_Throw()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition { Field = "Godliness", Operator = ComparisonOperator.GreaterThan, Value = 200 },
                    new MetaQueryCondition { Field = nameof(Person.Name), Operator = ComparisonOperator.Equals, Value = "Seb" })
                {
                    Operator = LogicalOperator.And
                };

                //Act
                var action = () => Database.People.Where(predicate);

                //Assert
                action.Should().Throw<ArgumentException>().WithParameterName("propertyName");
            }

            [TestMethod]
            public void WhenPredicateReferencesFieldOnChildObject_PerformSearchAccordingly()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition { Field = nameof(Person.Age), Operator = ComparisonOperator.LessThan, Value = 40 },
                    new MetaQueryCondition { Field = nameof(Person.Job), Operator = ComparisonOperator.NotEquals, Value = null },
                    new MetaQueryCondition { Field = $"{nameof(Person.Job)}.{nameof(Job.Title)}", Operator = ComparisonOperator.Equals, Value = "Engineer" })
                {
                    Operator = LogicalOperator.And
                };

                //Act
                var result = Database.People.Where(predicate).ToList();

                //Assert
                result.Should().BeEquivalentTo(new List<Person> { Database.People.Single(x => x.Id == 6) });
            }

            [TestMethod]
            public void WhenPredicateHasNoNodes_ReturnEmpty()
            {
                //Arrange
                var predicate = new MetaQueryPredicate() { Operator = Fixture.Create<LogicalOperator>() };

                //Act
                var result = Database.People.Where(predicate);

                //Assert
                result.Should().BeEmpty();
            }

            [TestMethod]
            public void WhenPredicateHasValidNodesButChildWithNoNodes_IgnoreEmptyNodesAndReturnAccordingly()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition { Field = nameof(Person.Age), Operator = ComparisonOperator.GreaterThanOrEquals, Value = 30, }, new MetaQueryPredicate { Operator = Fixture.Create<LogicalOperator>() })
                {
                    Operator = LogicalOperator.BitwiseAnd
                };

                //Act
                var result = Database.People.Where(predicate);

                //Assert
                result.Should().BeEquivalentTo(Database.People.Where(x => x.Age >= 30));
            }

            [TestMethod]
            public void WhenIsInOperatorOnValueThatIsNotCollection_Throw()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition { Field = nameof(Person.Name), Operator = ComparisonOperator.IsIn, Value = Fixture.Create<string>() })
                {
                    Operator = Fixture.Create<LogicalOperator>()
                };

                //Act
                var action = () => Database.People.Where(predicate);

                //Assert
                action.Should().Throw<ArgumentException>();
            }

            [TestMethod]
            public void WhenIsInOperatorOnValueThatIsCollection_ReturnAccordingly()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition { Field = nameof(Person.Name), Operator = ComparisonOperator.IsIn, Value = new List<string> { "Carol", "Stefan", "Coral" } })
                {
                    Operator = Fixture.Create<LogicalOperator>()
                };

                //Act
                var result = Database.People.Where(predicate);

                //Assert
                result.Should().BeEquivalentTo(Database.People.Where(x => x.Name == "Carol"));
            }

            [TestMethod]
            public void WhenIsNotInOperatorOnValueThatIsNotCollection_Throw()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition { Field = nameof(Person.Name), Operator = ComparisonOperator.IsNotIn, Value = Fixture.Create<string>() })
                {
                    Operator = Fixture.Create<LogicalOperator>()
                };

                //Act
                var action = () => Database.People.Where(predicate);

                //Assert
                action.Should().Throw<ArgumentException>();
            }

            [TestMethod]
            public void WhenIsNotInOperatorOnValueThatIsCollection_ReturnAccordingly()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition { Field = nameof(Person.Name), Operator = ComparisonOperator.IsNotIn, Value = new List<string> { "Carol", "Stefan", "Coral" } })
                {
                    Operator = Fixture.Create<LogicalOperator>()
                };

                //Act
                var result = Database.People.Where(predicate);

                //Assert
                result.Should().BeEquivalentTo(Database.People.Where(x => x.Name != "Carol"));
            }

            [TestMethod]
            public void WhenOneNodeIsUnrecognizedType_Throw()
            {
                //Arrange
                var predicate = new MetaQueryPredicate(new MetaQueryCondition
                {
                    Field = nameof(Person.Name),
                    Operator = ComparisonOperator.IsNotIn,
                    Value = new List<string> { "Carol", "Stefan", "Coral" }
                },
                    Fixture.Create<BogusNode>())
                {
                    Operator = Fixture.Create<LogicalOperator>()
                };

                //Act
                var action = () => Database.People.Where(predicate);

                //Assert
                action.Should().Throw<NotSupportedException>().WithMessage(string.Format(Exceptions.TypeUnsupportedByParser, nameof(BogusNode)));
            }
        }
    }
}