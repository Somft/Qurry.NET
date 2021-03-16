using System.Linq.Expressions;

namespace Qurry.Core
{
    public interface IPropertyResolver
    {
        ParameterExpression ResolveParameter<T>();

        Expression? ResolveProperty<T>(string propertyName, bool allowNesting = false);
    }
}