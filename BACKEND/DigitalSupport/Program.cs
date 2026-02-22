using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApiDigitalSupport.BusinessLogic;
using WebApiDigitalSupport.BusinessLogic.Interface;
using WebApiDigitalSupport.Domain;
using WebApiDigitalSupport.Repository.Interface;
using WebApiDigitalSupport.Repository.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión
builder.Services.AddDbContext<DigitalSupportContextDB>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("cnDigitalSupport"))
);

// Registro de servicios (inyección de dependencias)
builder.Services.AddScoped<IDigitalSupportRepository, DigitalSupportRepository>();
builder.Services.AddScoped<IDigitalSupport, DigitalSupportBL>();

// Agrega soporte para API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// ✅ Agrega CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Agrega Servicios para el contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// ✅ Aplica la política CORS aquí
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();
app.Run();