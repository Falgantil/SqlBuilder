using SqlBuilder.Core.Helpers;

namespace SqlBuilder.Core.Statements.Select
{
    public class SelectSqlOptions : SqlValueOptions
    {
        public bool AllowMissingWhere { get; set; }
    }
}