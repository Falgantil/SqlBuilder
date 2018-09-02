using System;
using System.Collections.Generic;
using System.Text;

using SqlBuilder.Core.Exceptions;

namespace SqlBuilder.Core.Statements.Delete
{
    public class DeleteSqlStatement : SqlStatement
    {
        public DeleteSqlStatement()
        {
        }

        public DeleteSqlStatement(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName { get; set; }

        public override void ValidateQuery()
        {
            if (string.IsNullOrEmpty(this.TableName))
            {
                throw new InvalidSqlStatementException("Missing table name");
            }
            if (this.Options?.AllowMissingWhere != true && this.Where == null)
            {
                throw new InvalidSqlStatementException("Missing WHERE statement");
            }
        }

        public WhereCondition Where { get; set; }

        public DeleteSqlOptions Options { get; set; } = new DeleteSqlOptions();

        public override string GenerateQuery()
        {
            this.ValidateQuery();

            var builder = new StringBuilder();
            builder.Append($"DELETE FROM {this.TableName}");
            if (this.Where != null)
            {
                builder.AppendLine();
                builder.Append(this.Where.GenerateQuery());
            }

            builder.Append(";");

            return builder.ToString();
        }
    }
}
