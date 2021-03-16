using System;
using System.Linq.Expressions;

namespace Qurry.Core.Query
{
    internal class QueryImplementation : IQuery
    {
        public IQueryNode Root { get; }

        public QueryImplementation(IQueryNode root)
        {
            this.Root = root;
        }

        public Expression<Func<T, bool>> ToExpression<T>(IPropertyResolver propertyResolver)
        {
            ParameterExpression parameter = propertyResolver.ResolveParameter<T>();

            return Expression.Lambda<Func<T, bool>>(this.Root.ToExpression<T>(propertyResolver), parameter);
        }
    }
}