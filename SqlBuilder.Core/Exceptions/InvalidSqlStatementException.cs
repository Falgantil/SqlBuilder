using System;
using System.Collections.Generic;
using System.Text;

namespace SqlBuilder.Core.Exceptions
{
    public class InvalidSqlStatementException : InvalidOperationException
    {
        public InvalidSqlStatementException()
        {
        }

        public InvalidSqlStatementException(string message)
            : base(message)
        {
        }
    }
}
