using ApiCatalogoController.Extensions;
using ApiCatalogoController.Services;
using ApiCatalogoController.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMySqlDbContext(builder.Configuration);

builder.Services.AddSingleton<IJwtService>(new JwtService());
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAppCors();

app.MapControllers();

app.Run();
