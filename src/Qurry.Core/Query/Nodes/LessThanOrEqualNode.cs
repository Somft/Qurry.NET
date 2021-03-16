using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public class LessThanOrEqualNode : OperatorNode
    {
        public LessThanOrEqualNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.LessThanOrEqual(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
