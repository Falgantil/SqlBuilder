// ReSharper disable StyleCop.SA1126

using System.Collections.Generic;

using SqlBuilder.Core.Helpers;

namespace SqlBuilder.Core.Statements
{
    public class InsertSqlOptions : SqlValueOptions
    {
        public List<int> SkipFormatRows { get; set; } = new List<int>();
    }
}