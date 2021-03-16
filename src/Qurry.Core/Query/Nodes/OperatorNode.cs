using System;
using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public abstract class OperatorNode : IQueryNode
    {
        public bool IsConstant => this.Left.IsConstant && this.Right.IsConstant;

        public IQueryNode Left { get; }
        public IQueryNode Right { get; }

        public OperatorNode(IQueryNode left, IQueryNode right)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public abstract Expression ToExpression<T>(IPropertyResolver propertyResolver);
    }
}
