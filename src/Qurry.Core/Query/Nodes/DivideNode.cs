using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    public class DivideNode : OperatorNode
    {
        public DivideNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.Divide(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
