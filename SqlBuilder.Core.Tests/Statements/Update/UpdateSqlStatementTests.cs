using System;
using System.Collections.Generic;

using Shouldly;

using SqlBuilder.Core.Exceptions;
using SqlBuilder.Core.Statements;
using SqlBuilder.Core.Statements.Update;

using Xunit;

namespace SqlBuilder.Core.Tests.Statements.Update
{
    public class UpdateSqlStatementTests
    {
        [Fact]
        public void VerifyCanGenerateWithoutWhere()
        {
            // Arrange
            var statement = new UpdateSqlStatement("People");
            statement.SetValues.Add(new KeyValuePair<string, object>("IsDead", true));
            statement.Options.AllowMissingWhere = true;

            // Act
            var query = statement.GenerateQuery();

            // Assert
            query.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void VerifyCantGenerateWithoutWhere()
        {
            // Arrange
            var statement = new UpdateSqlStatement("People");
            statement.SetValues.Add(new KeyValuePair<string, object>("IsDead", true));

            // Act & Assert
            Should.Throw<InvalidSqlStatementException>(() => statement.GenerateQuery());
        }

        [Fact]
        public void VerifyGenericUpdate()
        {
            // Arrange
            var statement = new UpdateSqlStatement("People");
            statement.SetValues.Add(new KeyValuePair<string, object>("IsBurried", true));
            statement.SetValues.Add(new KeyValuePair<string, object>("Location", "Some cemetary adress"));
            statement.Where = new WhereCondition()
                .AddSegment(new WhereCondition.Condition("Age", WhereCondition.ComparisonType.EqualToOrGreaterThan, 100))
                .AddSegment(WhereCondition.Operator.And)
                .AddSegment(new WhereCondition.Condition("IsDead", WhereCondition.ComparisonType.Equal, true));

            // Act
            var query = statement.GenerateQuery();

            // Assert
            var expected = $@"
UPDATE People
SET IsBurried=1, Location='Some cemetary adress'
WHERE (Age >= 100) AND (IsDead = 1);
";
            foreach (var c in Environment.NewLine)
                expected = expected.Trim(c);
            foreach (var c in Environment.NewLine)
                expected = expected.Trim(c);

            query.ShouldBe(expected);
        }
    }
}