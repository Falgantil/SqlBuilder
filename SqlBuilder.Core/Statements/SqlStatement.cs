namespace SqlBuilder.Core.Statements
{
    public abstract class SqlStatement
    {
        public abstract string GenerateQuery();
    }
}