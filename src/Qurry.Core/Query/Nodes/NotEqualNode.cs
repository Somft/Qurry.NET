using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public class NotEqualNode : OperatorNode
    {
        public NotEqualNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.NotEqual(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
