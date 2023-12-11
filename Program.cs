using ApiCatalogoController.Extensions;
using ApiCatalogoController.Logging;
using ApiCatalogoController.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMySqlDbContext(builder.Configuration);

builder.Services.AddSingleton<IJwtService>(new JwtService());
builder.Services.AddSingleton(builder.Configuration);

ILoggerFactory loggerFactory = new LoggerFactory();
loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAppCors();

app.MapControllers();

app.Run();
