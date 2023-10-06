using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Services.CategoriaService;
using Sibe.API.Services.ComponenteService;
using Sibe.API.Services.EquipoService;
using System.Text.Json.Serialization;
using Sibe.API.Models.Enums;
using Sibe.API.Services.EstadoService;
using JwtAuthenticationHandler;
using App.API.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Sibe.API.Services.UsuarioService;

var builder = WebApplication.CreateBuilder(args);

// Agregar la configuraci�n desde /Services/serviceMessages.json
builder.Configuration
    .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Services"))
    .AddJsonFile("servicesMessages.json", optional: true, reloadOnChange: true);

// Add DB with connection
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection") ??
        throw new InvalidOperationException("MySqlConnection no se ha encontrado.");
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.29-mysql"),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure();
        });
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new CustomEnumNamingPolicy()));
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Se inyecta el servicio de HttpContextAccesor para mover el uso de ids a partir del token al servicio en vez del controller
// esto porque vamos a necesitar siempre el id del current user para cada operacion del crud
builder.Services.AddHttpContextAccessor();

// Agrega la autenticaci�n JWT
builder.Services.AddSingleton(new JwtCredentialProvider(builder.Configuration));

// Registra la autenticaci�n JWT y pasa IConfiguration
builder.Services.AddJwtAuthentication(builder.Configuration);

// Inyeccion de servicios
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IEstadoService, EstadoService>();
builder.Services.AddScoped<IComponenteService, ComponenteService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Service to implement authentication middleware
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost3000",
            builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowLocalhost3000"); // Usar la política de CORS

app.MapControllers();


app.Run();
