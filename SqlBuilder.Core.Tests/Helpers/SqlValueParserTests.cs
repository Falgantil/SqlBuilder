using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Shouldly;
using SqlBuilder.Core.Helpers;
using Xunit;

namespace SqlBuilder.Core.Tests.Helpers
{
    public class SqlValueParserTests
    {
        [Fact]
        public void VerifyArrayParseWorks()
        {
            var values = new List<string>
            {
                "fisk 1",
                "fisk 2"
            };
            var value = SqlValueParser.ParseValue(values.ToArray(), new SqlValueOptions());
            value.ShouldBe("('fisk 1', 'fisk 2')");
        }

        [Fact]
        public void VerifyEnumerableParseWorks()
        {
            var values = new List<string>
            {
                "fisk 1",
                "fisk 2"
            };
            var value = SqlValueParser.ParseValue(values, new SqlValueOptions());
            value.ShouldBe("('fisk 1', 'fisk 2')");
        }
    }
}