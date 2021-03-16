using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public class GreaterThanNode : OperatorNode
    {
        public GreaterThanNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.GreaterThan(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
