using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Qurry.Core.Query.Nodes
{
    public class ValueNode : IQueryNode
    {
        public enum ValueType
        {
            Bool,
            Integer,
            Float,
            String,
            Unknown,
        }

        private static readonly Regex isBool = new Regex("^(true|false)$");
        private static readonly Regex isInteger = new Regex("^[0-9]+$");
        private static readonly Regex isFloat = new Regex("^[0-9.]+$");
        private static readonly Regex isString = new Regex("^'.*'$|^\".*\"$");

        public ValueType Type { get; } = ValueType.Unknown;

        public bool IsConstant { get; } = true;

        public string Value { get; }

        public ValueNode(string value)
        {
            this.Value = value ?? throw new System.ArgumentNullException(nameof(value));

            if (isBool.IsMatch(value.ToLower()))
            {
                this.Type = ValueType.Bool;
            }
            else if (isInteger.IsMatch(value))
            {
                this.Type = ValueType.Integer;
            }
            else if (isFloat.IsMatch(value))
            {
                this.Type = ValueType.Float;
            }
            else if (isString.IsMatch(value))
            {
                this.Value = this.Value[1..^1];
                this.Type = ValueType.String;
            }   
            else
            {
                this.IsConstant = false;
            } 
        }

        public Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            if (this.Type == ValueType.Bool)
            {
                return Expression.Constant(bool.Parse(this.Value.ToLower()));
            }
            else if (this.Type == ValueType.Integer)
            {
                return Expression.Constant(int.Parse(this.Value.ToLower()));
            }
            else if (this.Type == ValueType.Float)
            {
                return Expression.Constant(float.Parse(this.Value.ToLower(), CultureInfo.InvariantCulture));
            }
            else if (this.Type == ValueType.String)
            {
                return Expression.Constant(this.Value);
            }

            return propertyResolver.ResolveProperty<T>(this.Value, true) ?? throw new InvalidQueryException();
        }
    }
}