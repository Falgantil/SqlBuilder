using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SqlBuilder.Core.Exceptions;
using SqlBuilder.Core.Helpers;

namespace SqlBuilder.Core.Statements.Update
{
    public class UpdateSqlStatement : SqlStatement
    {
        public UpdateSqlStatement()
        {
        }

        public UpdateSqlStatement(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName { get; set; }

        public List<KeyValuePair<string, object>> SetValues { get; set; } = new List<KeyValuePair<string, object>>();

        public WhereCondition Where { get; set; }

        public override void ValidateQuery()
        {
            if (string.IsNullOrEmpty(this.TableName))
            {
                throw new InvalidSqlStatementException("Missing table name");
            }
            if (!this.SetValues.Any())
            {
                throw new InvalidSqlStatementException("Missing SET values");
            }
            if (this.Options?.AllowMissingWhere != true && this.Where == null)
            {
                throw new InvalidSqlStatementException("Missing WHERE statement");
            }
        }

        public UpdateSqlOptions Options { get; } = new UpdateSqlOptions();

        public override string GenerateQuery()
        {
            this.ValidateQuery();

            var builder = new StringBuilder();
            builder.AppendLine($"UPDATE {this.TableName}");
            var parsedValues = this.SetValues.Select(pair => $"{pair.Key}={SqlValueParser.ParseValue(pair.Value, this.Options)}").ToArray();
            builder.Append($"SET {string.Join(", ", parsedValues)}");
            if (this.Where != null)
            {
                builder.AppendLine();
                builder.Append(this.Where.GenerateQuery());
            }

            builder.Append(";");

            return builder.ToString();
        }
    }

    public class UpdateSqlOptions : SqlValueOptions
    {
        public bool AllowMissingWhere { get; set; }
    }
}
