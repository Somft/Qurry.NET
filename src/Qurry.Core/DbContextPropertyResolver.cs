using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Qurry.Core
{
    public class DbContextPropertyResolver<TDbContext> : IPropertyResolver where TDbContext : class
    {
        protected Dictionary<Type, ParameterExpression> Parameters { get; set; }
        protected Dictionary<Type, Dictionary<string, MemberExpression>> Properties { get; }

        protected List<Type> SupportedTypes = new List<Type>
        {
            typeof(int),
            typeof(long),
            typeof(string),
            typeof(bool),
        };

        public DbContextPropertyResolver()
        {
            this.Parameters = typeof(TDbContext).GetProperties()
                .Select(p => p.PropertyType)
                .Where(pt => pt.IsGenericType)
                .Where(pt => pt.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(pt => pt.GetGenericArguments().FirstOrDefault())
                .ToDictionary(pt => pt, pt => Expression.Parameter(pt, pt.FullName));
            
            this.SupportedTypes.AddRange(Parameters.Keys);

            this.Properties = this.Parameters
                .ToDictionary(pt => pt.Key, pt => pt.Key
                    .GetProperties()
                    .Where(p => this.SupportedTypes.Contains(p.PropertyType))
                    .ToDictionary(p => p.Name.ToLowerInvariant(), p => Expression.Property(pt.Value, p)));
        }

        public Expression? ResolveProperty<T>(string propertyName, bool allowNesting)
        {
            Dictionary<string, MemberExpression> fields = this.Properties[typeof(T)]
                ?? throw new InvalidOperationException();

            if (!propertyName.Contains(".") || !allowNesting)
            {
                return fields[propertyName.ToLowerInvariant()];
            }
            
            int split = propertyName.LastIndexOf('.');
            string prefix = propertyName[..split];
            string suffix = propertyName[(split + 1)..];
            Expression? subProperty = ResolveProperty<T>(prefix, true);

            return subProperty == null ? null : Expression.PropertyOrField(subProperty, suffix);
        }

        public ParameterExpression ResolveParameter<T>()
        {
            return this.Parameters[typeof(T)] ?? throw new InvalidOperationException();
        }
    }
}
