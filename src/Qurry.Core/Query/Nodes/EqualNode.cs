using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public class EqualNode : OperatorNode
    {
        public EqualNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            (Expression left, Expression right) = this.TryAlignTypes<T>(propertyResolver);
            return Expression.Equal(left, right);
        }
    }
}
