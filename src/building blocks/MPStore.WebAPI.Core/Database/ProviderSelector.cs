using Microsoft.EntityFrameworkCore;
using static MPStore.WebAPI.Core.Database.ProviderConfiguration;

namespace MPStore.WebAPI.Core.Database
{
    public static class ProviderSelector
    {
        public static IServiceCollection ConfigureProviderForContext<TContext>(
            this IServiceCollection services,
            (DatabaseType, string) options) where TContext : DbContext
        {
            var (database, connString) = options;

            return database switch
            {
                DatabaseType.SqlServer => services.PersistStore<TContext>(Build(connString).With().SqlServer),

                _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
            };
        }

        public static Action<DbContextOptionsBuilder> WithProviderAutoSelection((DatabaseType, string) options)
        {
            var (database, connString) = options;
            return database switch
            {
                DatabaseType.SqlServer => Build(connString).With().SqlServer,
                _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
            };
        }
    }
}
