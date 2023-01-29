using Microsoft.OpenApi.Models;
using Pho.Core;
using Pho.Infrastructure;
using Pho.Web.Middleware;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddTransient<ExceptionMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Potentially Hazardous Object (PHO) API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(filePath);
});

builder.Services.AddCoreServices();
builder.Services.AddNasaNeoService(builder.Configuration.GetSection("NasaNeoService"));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(); // This is for unhandled exception
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
