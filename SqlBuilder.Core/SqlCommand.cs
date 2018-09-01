using System;
using System.Text;

using SqlBuilder.Core.Statements;

namespace SqlBuilder.Core
{
    public class SqlCommand
    {
        private readonly StringBuilder builder = new StringBuilder();
        
        public void AppendStatement(SqlStatement statement)
        {
            this.builder.AppendLine(statement.GenerateQuery());
        }

        public void AppendRawSql(string sql)
        {
            this.builder.AppendLine(sql);
        }

        public string GenerateQuery()
        {
            return this.builder.ToString();
        }
    }
}
