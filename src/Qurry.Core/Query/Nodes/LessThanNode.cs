using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public class LessThanNode : OperatorNode
    {
        public LessThanNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            (Expression left, Expression right) = this.TryAlignTypes<T>(propertyResolver);
            return Expression.LessThan(left, right);
        }
    }
}
