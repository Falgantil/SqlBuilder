using Shouldly;
using SqlBuilder.Core.Exceptions;
using SqlBuilder.Core.Statements;
using Xunit;

namespace SqlBuilder.Core.Tests.Statements
{
    public class WhereConditionTests
    {
        [Fact]
        public void VerifyAllowedTrailingOperatorsGetsTrimmed()
        {
            // Arrange
            var condition = new WhereCondition
            {
                Options = new WhereSqlOptions
                {
                    StripTrailingOperators = true
                }
            };
            condition.AddSegment(new WhereCondition.Condition("Entity", WhereCondition.ComparisonType.Equal, 42));
            condition.AddSegment(WhereCondition.Operator.And);
            

            // Act
            var query = condition.GenerateQuery();


            // Assert
            query.ShouldBe("WHERE (Entity = 42)");
        }

        [Fact]
        public void VerifyNotAllowedTrailingOperatorsThrowsException()
        {
            // Arrange
            var condition = new WhereCondition();
            condition.AddSegment(new WhereCondition.Condition("Entity", WhereCondition.ComparisonType.Equal, 42));
            condition.AddSegment(WhereCondition.Operator.And);


            // Act & Assert
            Should.Throw<InvalidSqlStatementException>(() => condition.GenerateQuery());
        }
    }
}