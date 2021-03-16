using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Qurry.Core.Query;

namespace Qurry.Core
{
    public static class IServiceCollectionExtesions
    {
        public static void AddExpressionParser<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddTransient<IExpressionParser, ExpressionParser>();
            services.AddTransient<IQueryParser, QueryParser>();
            services.AddTransient<IPropertyResolver, DbContextPropertyResolver<T>>();
        }
    }
}
