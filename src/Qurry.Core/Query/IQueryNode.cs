using System.Linq.Expressions;

namespace Qurry.Core.Query
{
    public interface IQueryNode
    {
        bool IsConstant { get; }

        Expression ToExpression<T>(IPropertyResolver propertyResolver);
    }
}
