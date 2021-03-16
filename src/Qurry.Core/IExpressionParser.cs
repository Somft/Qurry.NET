using System;
using System.Linq.Expressions;

namespace Qurry.Core
{
    public interface IExpressionParser
    {
        Expression<Func<T, bool>> ParseExpression<T>(string expression);
    }
}