using Microsoft.OpenApi.Models;
using GarageService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    // Define the Swagger document
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Garage Service APIs",
        Description = "Garage Service APIs",
    });

    // Add Swagger annotations (if desired)
    c.EnableAnnotations();
});

// Add services to the container.
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

startup.Configure(app, builder.Environment);

app.MapControllers();

app.Run();
