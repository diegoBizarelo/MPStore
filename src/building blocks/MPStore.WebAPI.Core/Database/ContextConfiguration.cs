using Microsoft.EntityFrameworkCore;

namespace MPStore.WebAPI.Core.Database
{
    public static class ContextConfiguration
    {
        public static IServiceCollection PersistStore<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> databaseConfig) where TContext : DbContext
        {
            if (services.All(x => x.ServiceType != typeof(TContext))) 
            {
                services.AddDbContext<TContext>(databaseConfig);
            }
            return services;
        }
    }
}
