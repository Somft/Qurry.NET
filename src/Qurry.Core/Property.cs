using System;
using System.Linq.Expressions;

namespace Qurry.Core
{
    public class Property : IProperty
    {
        public Expression Expression { get; }
        public Type ReturnType { get; }

        public Property(Expression expression, Type returnType)
        {
            this.Expression = expression;
            this.ReturnType = returnType;
        }
    }
}