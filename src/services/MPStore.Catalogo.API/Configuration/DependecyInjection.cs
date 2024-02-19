using MPStore.Catalogo.API.Data;
using MPStore.Catalogo.API.Data.Repository;
using MPStore.Catalogo.API.Models;

namespace MPStore.Catalogo.API.Configuration
{
    public static class DependecyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContexto>();
        }
    }
}
