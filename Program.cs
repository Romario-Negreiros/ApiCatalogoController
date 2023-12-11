using ApiCatalogoController.Extensions;
using ApiCatalogoController.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMySqlDbContext(builder.Configuration);

builder.Services.AddSingleton<IJwtService>(new JwtService());
builder.Services.AddSingleton(builder.Configuration);

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAppCors();

app.MapControllers();

app.Run();
