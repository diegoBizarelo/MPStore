using MPStore.Cliente.API.Configuration;
using MPStore.WebAPI.Core.Identity;
using Serilog;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddSerilog(new LoggerConfiguration().
    ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.RegisterServices();

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.Run();
