using Microsoft.EntityFrameworkCore;
using Sibe.API.Data;
using Sibe.API.Services.CategoriaService;
using Sibe.API.Services.ComponenteService;
using Sibe.API.Services.EquipoService;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;
using Sibe.API.Models.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add DB with connection
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection") ??
        throw new InvalidOperationException("MySqlConnection no se ha encontrado.");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new CustomEnumNamingPolicy()));
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyeccion de servicios
builder.Services.AddScoped<IEquipoService, EquipoService>();
//builder.Services.AddScoped<IComponenteService, ComponenteService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
