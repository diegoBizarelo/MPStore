using MPStore.Identidade.API.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger());

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.MapControllers();

app.Run();