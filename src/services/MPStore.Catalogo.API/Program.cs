using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.AddSerilog(new LoggerConfiguration().
    ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
