using System;
using System.Collections.Generic;
using System.Text;
using SqlBuilder.Core.Exceptions;

namespace SqlBuilder.Core.Statements
{
    public class JoinCondition : SqlStatement
    {
        public string TableName { get; set; }
        public string Comparison { get; set; }

        public JoinCondition(string tableName, string comparison)
        {
            TableName = tableName;
            Comparison = comparison;
        }

        public JoinCondition()
        {
            
        }

        public override void ValidateQuery()
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new InvalidSqlStatementException("Missing join table name");
            }
            if (string.IsNullOrEmpty(Comparison))
            {
                throw new InvalidSqlStatementException("Missing join table comparison");
            }
        }

        public override string GenerateQuery()
        {
            this.ValidateQuery();

            return $"Join {TableName} On {Comparison}";
        }
    }
}
