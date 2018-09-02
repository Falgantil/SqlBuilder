using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SqlBuilder.Core.Exceptions;
using SqlBuilder.Core.Helpers;

namespace SqlBuilder.Core.Statements.Select
{
    public class SelectSqlStatement : SqlStatement
    {
        public SelectSqlStatement()
        {
        }

        public SelectSqlStatement(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName { get; set; }

        public List<string> Columns { get; set; }

        public WhereCondition Where { get; set; }

        public OrderByCondition OrderBy { get; set; }

        public SelectSqlOptions Options { get; set; } = new SelectSqlOptions();

        public override void ValidateQuery()
        {
            if (string.IsNullOrEmpty(this.TableName))
            {
                throw new InvalidSqlStatementException("Missing table name");
            }
            if (this.Columns.Count < 1)
            {
                throw new InvalidSqlStatementException("Missing columns");
            }
            if (this.Options?.AllowMissingWhere != true && this.Where == null)
            {
                throw new InvalidSqlStatementException("Missing WHERE statement");
            }
        }

        public override string GenerateQuery()
        {
            this.ValidateQuery();

            var builder = new StringBuilder();
            builder.AppendLine($"SELECT {string.Join(", ", this.Columns)}");
            builder.Append($"FROM {this.TableName}");
            if (this.Where != null)
            {
                builder.AppendLine();
                builder.Append(this.Where.GenerateQuery());
            }
            if (this.OrderBy != null)
            {
                builder.AppendLine();
                builder.Append(this.OrderBy.GenerateQuery());
            }

            builder.Append(";");

            return builder.ToString();
        }
    }

    public class SelectSqlOptions : SqlValueOptions
    {
        public bool AllowMissingWhere { get; set; }
    }
}
