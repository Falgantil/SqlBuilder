using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SqlBuilder.Core.Exceptions;

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

        public List<JoinCondition> Joins { get; set; } = new List<JoinCondition>();

        public List<string> Columns { get; set; } = new List<string>();

        public WhereCondition Where { get; set; }

        public OrderByCondition OrderBy { get; set; }

        public SelectSqlOptions Options { get; set; } = new SelectSqlOptions();

        public override void ValidateQuery()
        {
            if (string.IsNullOrEmpty(this.TableName))
            {
                throw new InvalidSqlStatementException("Missing table name");
            }
            if (this.Columns == null || this.Columns.Count < 1)
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
            builder.Append("SELECT ");
            if (this.Options?.Distinct == true)
            {
                builder.Append("DISTINCT ");
            }
            builder.AppendLine($"{string.Join(", ", this.Columns)}");
            builder.Append($"FROM {this.TableName}");
            foreach (var join in Joins)
            {
                builder.AppendLine(join.GenerateQuery());
            }
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
}
