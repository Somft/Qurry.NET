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

        protected (Expression, Expression) TryAlignTypes<T>(IPropertyResolver propertyResolver)
        {
            if (this.Left.IsConstant && this.Right.IsConstant)
            {

            }
            else if (this.Left.IsConstant)
            {
                (Expression left, Expression right) = this.AlignTypesWithSingleConstant<T>(this.Left, this.Right, propertyResolver);
                return (left, right);
            }
            else if (this.Right.IsConstant)
            {
                (Expression right, Expression left) = this.AlignTypesWithSingleConstant<T>(this.Right, this.Left, propertyResolver);
                return (left, right);
            }

            return (this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }

        protected (Expression, Expression) AlignTypesWithSingleConstant<T>(IQueryNode constant, IQueryNode other, IPropertyResolver propertyResolver)
        {
            var otherExpression = other.ToExpression<T>(propertyResolver);
            Type? otherType = otherExpression.Type;

            if (otherType == typeof(DateTime) && constant is ValueNode constantValue && constantValue.Type == ValueNode.ValueType.String)
            {
                return (Expression.Constant(DateTime.Parse(constantValue.Value)), otherExpression);
            }

            return (constant.ToExpression<T>(propertyResolver), otherExpression);
        }
    }
}
