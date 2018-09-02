using System;
using System.Collections.Generic;
using System.Text;

using SqlBuilder.Core.Helpers;

namespace SqlBuilder.Core.Statements.Delete
{
    public class DeleteSqlOptions : SqlValueOptions
    {
        public bool AllowMissingWhere { get; set; }
    }
}
