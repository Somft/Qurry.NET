using System.Linq.Expressions;

namespace Qurry.Core.Query.Nodes
{
    internal class AddNode : OperatorNode
    {
        public AddNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.Add(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
