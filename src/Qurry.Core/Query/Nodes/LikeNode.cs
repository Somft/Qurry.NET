using Microsoft.EntityFrameworkCore;

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Qurry.Core.Query.Nodes
{
    internal class LikeNode : OperatorNode
    {
        private static readonly ConstantExpression functions = Expression.Constant(EF.Functions);

        private static readonly MethodInfo? likeMethod = typeof(DbFunctionsExtensions)
            .GetMethod("Like", new[]
            {
                typeof(DbFunctions),
                typeof(string),
                typeof(string)
            });

        public LikeNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.Call(
                instance: null,
                likeMethod ?? throw new Exception(),
                functions, this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
