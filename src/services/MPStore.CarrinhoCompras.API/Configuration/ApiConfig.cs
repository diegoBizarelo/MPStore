using MPStore.CarrinhoCompras.API.Data;
using MPStore.CarrinhoCompras.API.Services.gRPC;
using static MPStore.WebAPI.Core.Database.ProviderSelector;
using static MPStore.WebAPI.Core.Database.ProviderConfiguration;
using MPStore.WebAPI.Core.Configuration;

namespace MPStore.CarrinhoCompras.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureProviderForContext<ShoppingCartContext>(DetectDatabase(configuration));

            services.AddGrpc();

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            services.AddDefaultHealthCheck(configuration);
        }

        public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Under certain scenarios, e.g minikube / linux environment / behind load balancer
            // https redirection could lead dev's to over complicated configuration for testing purpouses
            // In production is a good practice to keep it true
            if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
                app.UseHttpsRedirection();

            app.UseCors("Total");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<CarrinhoComprasPedidosGrpcService>().RequireCors("Total");

            //app.UseDefaultHealthcheck();
        }
    }
}
