using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public class AndNode : OperatorNode
    {
        public AndNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.AndAlso(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
