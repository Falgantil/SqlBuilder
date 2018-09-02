namespace SqlBuilder.Core.Statements
{
    public abstract class SqlStatement
    {
        public abstract void ValidateQuery();
        public abstract string GenerateQuery();
    }
}