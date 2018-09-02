using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using SqlBuilder.Core.Exceptions;

namespace SqlBuilder.Core.Helpers
{
    public static class SqlValueParser
    {
        public static string ParseValue(object value, SqlValueOptions options)
        {
            if (value == null)
            {
                return "NULL";
            }

            switch (value)
            {
                case string s:
                    return $"'{s.Replace("'", "''")}'";
                case Enum e:
                    if (options?.InsertEnumsAsString == true)
                    {
                        return $"'{e.ToString("G")}'";
                    }
                    return Convert.ToInt32(value).ToString();
                case byte _:
                case sbyte _:
                case short _:
                case ushort _:
                case int _:
                case uint _:
                case long _:
                case ulong _:
                case float _:
                case double _:
                case decimal _:
                    if (options?.InsertNumberZeroAsNull == true)
                    {
                        return "NULL";
                    }

                    return value.ToString();
                case Array coll:
                    var items = new List<object>();
                    foreach (var item in coll)
                    {
                        items.Add(ParseValue(item, options));
                    }
                    return $"({string.Join(", ", coll)})";
                case bool b:
                    if (options?.InsertBoolAsText == true)
                    {
                        return b ? "'True'" : "'False'";
                    }

                    return b ? "1" : "0";
                default:
                    if (options?.AllowClassTypes != true && value.GetType().GetTypeInfo().IsClass)
                    {
                        throw new InvalidSqlStatementException($"Options does not permit class types. Culprit: {value.GetType().FullName}");
                    }
                    break;
            }

            return $"'{value}'";
        }
    }

    public class SqlValueOptions
    {
        public bool InsertEnumsAsString { get; set; }

        public bool InsertNumberZeroAsNull { get; set; }

        public bool InsertBoolAsText { get; set; }

        public bool AllowClassTypes { get; set; }
    }
}
