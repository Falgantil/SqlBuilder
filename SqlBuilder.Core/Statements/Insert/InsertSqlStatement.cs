using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using SqlBuilder.Core.Exceptions;
using SqlBuilder.Core.Helpers;

// ReSharper disable StyleCop.SA1126

namespace SqlBuilder.Core.Statements
{
    public class InsertSqlStatement : SqlStatement
    {
        public InsertSqlStatement()
        {

        }

        public InsertSqlStatement(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName { get; set; }

        public List<string> Columns { get; } = new List<string>();

        public List<List<object>> Rows { get; } = new List<List<object>>();

        public InsertSqlOptions Options { get; set; } = new InsertSqlOptions();

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
            if (this.Rows.Count < 1)
            {
                throw new InvalidSqlStatementException("Missing rows");
            }
            var differentCount = this.Rows.FirstOrDefault(row => row.Count != this.Columns.Count);
            if (differentCount != null)
            {
                throw new InvalidSqlStatementException($"A row had {differentCount} columns, compared to the Column count of {this.Columns.Count}");
            }

            var types = this.Rows[0].Select(rowData => rowData.GetType()).ToArray();
            foreach (var row in this.Rows)
            {
                for (var i = 0; i < row.Count; i++)
                {
                    if (types[i] != row[i].GetType())
                    {
                        throw new InvalidSqlStatementException("Inconsistent row data. Mixed types in columns!");
                    }
                }
            }
        }

        public override string GenerateQuery()
        {
            this.ValidateQuery();

            var builder = new StringBuilder();
            builder.AppendLine($"INSERT INTO {this.TableName} ({string.Join(", ", this.Columns)})");
            builder.Append("VALUES ");

            var values = this.Rows.Select(row => $"({string.Join(", ", row.Select(this.GetSqlValue))})").ToArray();

            builder.Append(string.Join($",{Environment.NewLine}", values));

            builder.Append(";");

            return builder.ToString();
        }

        private string GetSqlValue(object value, int index)
        {
            if (this.Options?.SkipFormatRows.Any(rowIndex => rowIndex == index) == true)
            {
                return value?.ToString();
            }

            return SqlValueParser.ParseValue(value, this.Options);
        }

        public static InsertSqlStatement Generate<T>(string tableName, IEnumerable<T> items)
        {
            var statement = new InsertSqlStatement(tableName);

            var itemType = typeof(T);
            var properties = itemType.GetRuntimeProperties().Where(i => i.CanRead).ToArray();

            foreach (var info in properties)
            {
                statement.Columns.Add(info.Name);
            }

            foreach (var item in items)
            {
                var row = properties.Select(info => info.GetValue(item)).ToList();
                statement.Rows.Add(row);
            }

            return statement;
        }
    }
}