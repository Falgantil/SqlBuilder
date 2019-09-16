using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SqlBuilder.Core.Exceptions;
using SqlBuilder.Core.Helpers;

namespace SqlBuilder.Core.Statements
{
    public class WhereCondition : SqlStatement
    {
        private readonly List<object> segments = new List<object>();

        public IEnumerable<object> Segments => this.segments;

        public override void ValidateQuery()
        {
            if (!this.Segments.Any())
            {
                throw new InvalidSqlStatementException("Missing segments in WHERE");
            }
        }

        public override string GenerateQuery()
        {
            this.ValidateQuery();

            var builder = new StringBuilder();
            builder.Append("WHERE");
            var selected = this.segments;

            if (!selected.Any())
            {
                throw new InvalidSqlStatementException("Invalid WHERE statement, missing at least 1 segment");
            }

            if (this.Options?.StripTrailingOperators == true)
            {
                while (selected.Last() is Operator)
                {
                    selected = selected.Take(selected.Count - 1).ToList();
                }
            }
            else if (selected.Last() is Operator lastOp)
            {
                throw new InvalidSqlStatementException($"Trailing Operator: {lastOp:G}");
            }

            if (!selected.Any())
            {
                throw new InvalidSqlStatementException("Invalid WHERE statement, missing at least 1 segment");
            }

            for (var i = 0; i < selected.Count; i++)
            {
                var segment = selected[i];
                if (segment is Condition condition)
                {
                    builder.Append(this.GenerateConditionSql(condition));
                    continue;
                }

                if (segment is Operator op)
                {
                    builder.Append(this.GenerateOperatorSql(op));
                    continue;
                }

                throw new InvalidSqlStatementException($"Invalid segment: {segment}");
            }

            return builder.ToString();
        }

        public WhereCondition AddSegment(Condition condition)
        {
            this.segments.Add(condition);
            return this;
        }

        public WhereCondition AddSegment(Operator op)
        {
            this.segments.Add(op);
            return this;
        }

        private string GenerateOperatorSql(Operator op)
        {
            switch (op)
            {
                case Operator.And:
                    return " AND";
                case Operator.Or:
                    return " OR";
                case Operator.Not:
                    return " NOT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }

        private string GenerateConditionSql(Condition condition)
        {
            var comparison = string.Empty;
            switch (condition.Comparison)
            {
                case ComparisonType.Equal:
                    comparison = "=";
                    break;
                case ComparisonType.NotEqual:
                    comparison = "<>";
                    break;
                case ComparisonType.Like:
                    comparison = "LIKE";
                    break;
                case ComparisonType.GreaterThan:
                    comparison = ">";
                    break;
                case ComparisonType.LesserThan:
                    comparison = "<";
                    break;
                case ComparisonType.EqualToOrGreaterThan:
                    comparison = ">=";
                    break;
                case ComparisonType.EqualToOrLesserThan:
                    comparison = "<=";
                    break;
                case ComparisonType.InArray:
                    comparison = "IN";
                    break;
                case ComparisonType.Is:
                    comparison = "IS";
                    break;
                case ComparisonType.IsNot:
                    comparison = "IS NOT";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return $" ({condition.Key} {comparison} {SqlValueParser.ParseValue(condition.Value, this.Options)})";
        }

        public WhereSqlOptions Options { get; set; }

        public class Condition
        {
            public Condition(string key, ComparisonType comparison, object value)
            {
                this.Key = key;
                this.Comparison = comparison;
                this.Value = value;
            }

            public string Key { get; set; }

            public object Value { get; set; }

            public ComparisonType Comparison { get; set; }
        }

        public enum Operator
        {
            Unknown,
            And,
            Or,
            Not
        }

        public enum ComparisonType
        {
            Unknown,
            Equal,
            NotEqual,
            Like,
            GreaterThan,
            LesserThan,
            EqualToOrGreaterThan,
            EqualToOrLesserThan,
            InArray,
            Is,
            IsNot
        }
    }

    public class WhereSqlOptions : SqlValueOptions
    {
        public bool StripTrailingOperators { get; set; }
    }

    public class OrderByCondition : SqlStatement
    {
        public List<Condition> Columns { get; set; } = new List<Condition>();

        public override void ValidateQuery()
        {
            if (!this.Columns.Any())
            {
                throw new InvalidSqlStatementException("Missing column name");
            }
        }

        public override string GenerateQuery()
        {
            var builder = new StringBuilder();

            var items = this.Columns.Select(c => $"{c.ColumnName} {(!c.ByDescending ? "ASC" : "DESC")}");
            builder.Append($"ORDER BY {string.Join(", ", items)}");

            return builder.ToString();
        }

        public class Condition
        {
            public string ColumnName { get; set; }

            public bool ByDescending { get; set; }
        }
    }
}
