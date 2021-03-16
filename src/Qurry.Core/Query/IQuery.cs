using System;
using System.Linq.Expressions;

namespace Qurry.Core.Query
{
    public interface IQuery
    {
        IQueryNode Root { get; }

        Expression<Func<T, bool>> ToExpression<T>(IPropertyResolver propertyResolver);
    }
}