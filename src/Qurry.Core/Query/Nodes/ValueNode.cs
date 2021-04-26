using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Qurry.Core.Query.Nodes
{
    public class ValueNode : IQueryNode
    {
        private readonly Regex isBool = new Regex("^(true|false)$");
        private readonly Regex isInteger = new Regex("^[0-9]+$");
        private readonly Regex isFloat = new Regex("^[0-9.]+$");
        private readonly Regex isString = new Regex("^'.*'$|^\".*\"$");

        public bool IsConstant => false;

        public string Value { get; }

        public ValueNode(string value)
        {
            this.Value = value ?? throw new System.ArgumentNullException(nameof(value));
        }

        public Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            if (this.isBool.IsMatch(this.Value.ToLower()))
            {
                return Expression.Constant(bool.Parse(this.Value.ToLower()));
            }
            else if (this.isInteger.IsMatch(this.Value))
            {
                return Expression.Constant(int.Parse(this.Value.ToLower()));
            }
            else if (this.isFloat.IsMatch(this.Value))
            {
                return Expression.Constant(float.Parse(this.Value.ToLower(), CultureInfo.InvariantCulture));
            }
            else if (this.isString.IsMatch(this.Value))
            {
                return Expression.Constant(this.Value[1..^1]);
            }

            return propertyResolver.ResolveProperty<T>(this.Value, true) ?? throw new InvalidQueryException();
        }
    }
}