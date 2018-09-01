// ReSharper disable StyleCop.SA1126

using System.Collections.Generic;

namespace SqlBuilder.Core.Statements
{
    public class InsertSqlOptions
    {
        public bool InsertEnumsAsString { get; set; }

        public bool InsertNumberZeroAsNull { get; set; }

        public bool InsertBoolAsNumbers { get; set; }

        public bool AllowClassTypes { get; set; }

        public List<int> SkipFormatRows { get; set; } = new List<int>();
    }
}