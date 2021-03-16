namespace Qurry.Core.Query
{
    public interface IQueryParser
    {
        IQuery Parse(string expression);
    }
}