using Qurry.Core.Query;

using System;
using System.Linq.Expressions;

namespace Qurry.Core
{
    public class ExpressionParser : IExpressionParser
    {
        private readonly IQueryParser queryParser;
        private readonly IPropertyResolver propertyResolver;

        public ExpressionParser(IQueryParser queryParser, IPropertyResolver propertyResolver)
        {
            this.queryParser = queryParser ?? throw new ArgumentNullException(nameof(queryParser));
            this.propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
        }

        public virtual Expression<Func<T, bool>> ParseExpression<T>(string expression)
        {
            try
            {
                IQuery query = this.queryParser.Parse(expression);
                return query.ToExpression<T>(this.propertyResolver);
            }
            catch
            {
                throw new InvalidQueryException();
            }
        }
    }
}
