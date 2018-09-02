using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shouldly;

using SqlBuilder.Core.Statements;

using Xunit;

namespace SqlBuilder.Core.Tests.Statements.Insert
{
    public class InsertSqlStatementTests
    {
        [Fact]
        public void VerifyGenerateRawData()
        {
            // Arrange
            var statement = new InsertSqlStatement("MagicTable");
            const string Column1 = "Column_1";
            const string Column2 = "Column_2";
            const string Column3 = "Magical_arbitrary_column";
            statement.Columns.AddRange(new[]
            {
                Column1,
                Column2,
                Column3
            });
            statement.Rows.AddRange(new[]
            {
                new List<object> { "Data 1", 24, true},
                new List<object> { "Cool 2", 42, false}
            });

            // Act
            var query = statement.GenerateQuery();

            // Assert
            var expected = $@"
INSERT INTO MagicTable ({Column1}, {Column2}, {Column3})
VALUES ('Data 1', 24, 1),
('Cool 2', 42, 0);
";
            foreach (var c in Environment.NewLine) expected = expected.Trim(c);
            foreach (var c in Environment.NewLine) expected = expected.Trim(c);

            query.ShouldBe(expected);
        }

        private class PersonModel
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }

            public PersonGender Gender { get; set; }

            public bool IsDead { get; set; }
        }

        [Fact]
        public void VerifyGenerateFromModel()
        {
            // Arrange
            var people = new[]
            {
                new PersonModel
                {
                    FirstName = "Johnny",
                    LastName = "Appleseed",
                    Age = 24,
                    Gender = PersonGender.Male,
                    IsDead = false
                }, 
                new PersonModel
                {
                    FirstName = "Alberta",
                    LastName = "Pearseed",
                    Age = 103,
                    Gender = PersonGender.Female,
                    IsDead = true
                }
            };
            
            // Act
            var statement = InsertSqlStatement.Generate("People", people);
            var query = statement.GenerateQuery();

            // Assert
            var expected = $@"
INSERT INTO People (FirstName, LastName, Age, Gender, IsDead)
VALUES ('Johnny', 'Appleseed', 24, 1, 0),
('Alberta', 'Pearseed', 103, 2, 1);
";
            foreach (var c in Environment.NewLine)
                expected = expected.Trim(c);
            foreach (var c in Environment.NewLine)
                expected = expected.Trim(c);

            query.ShouldBe(expected);
        }
    }
}
