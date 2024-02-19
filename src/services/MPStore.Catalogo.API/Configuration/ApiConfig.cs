using MPStore.Catalogo.API.Data;
using static MPStore.WebAPI.Core.Database.ProviderSelector;
using static MPStore.WebAPI.Core.Database.ProviderConfiguration;

namespace MPStore.Catalogo.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureProviderForContext<CatalogoContexto>(DetectDatabase(configuration));

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            //services.AddDefaultHealthCheck(configuration);
        }

        public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            //app.UseAuthConfiguration();

            app.MapControllers();

            //app.UseDefaultHealthcheck();
        }
    }
}
