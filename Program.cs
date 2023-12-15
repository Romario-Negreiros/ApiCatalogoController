using ApiCatalogoController.Extensions;
using ApiCatalogoController.Services;
using ApiCatalogoController.Repositories;
using AutoMapper;
using ApiCatalogoController.DTOs.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMySqlDbContext(builder.Configuration);

builder.Services.AddIdentity();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<IJwtService>(new JwtService(builder.Configuration));
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseAppCors();

app.MapControllers();

app.Run();
