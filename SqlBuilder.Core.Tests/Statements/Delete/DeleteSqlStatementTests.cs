using System;

using Shouldly;

using SqlBuilder.Core.Exceptions;
using SqlBuilder.Core.Statements;
using SqlBuilder.Core.Statements.Delete;
using SqlBuilder.Core.Tests.Statements.Insert;

using Xunit;

namespace SqlBuilder.Core.Tests.Statements.Delete
{
    public class DeleteSqlStatementTests
    {
        [Fact]
        public void VerifyCanDeleteWithoutWhere()
        {
            // Arrange
            var statement = new DeleteSqlStatement("People")
            {
                Options = { AllowMissingWhere = true }
            };

            // Act
            var query = statement.GenerateQuery();

            // Assert
            query.ShouldBe($"DELETE FROM People;");
        }

        [Fact]
        public void VerifyCantDeleteWithoutWhere()
        {
            // Arrange
            var statement = new DeleteSqlStatement("People")
            {
                Options = { AllowMissingWhere = false }
            };

            // Act & Assert
            Should.Throw<InvalidSqlStatementException>(() => statement.GenerateQuery());
        }

        [Fact]
        public void VerifyDeleteQuery()
        {
            // Arrange
            var statement = new DeleteSqlStatement("People")
            {
                Where = new WhereCondition()
                    .AddSegment(new WhereCondition.Condition("Age", WhereCondition.ComparisonType.GreaterThan, 150))
                    .AddSegment(WhereCondition.Operator.Or)
                    .AddSegment(new WhereCondition.Condition("Gender", WhereCondition.ComparisonType.Equal, PersonGender.Undefined))
            };

            // Act
            var query = statement.GenerateQuery();

            // Assert
            var expected = $@"
DELETE FROM People
WHERE (Age > 150) OR (Gender = 0);
";
            foreach (var c in Environment.NewLine)
                expected = expected.Trim(c);
            foreach (var c in Environment.NewLine)
                expected = expected.Trim(c);

            query.ShouldBe(expected);
        }
    }
}