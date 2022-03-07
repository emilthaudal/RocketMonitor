using System.Reflection;
using System.Text.Json;
using RocketMonitor.Infrastructure.Interface;
using RocketMonitor.MemoryEventStore;
using RocketMonitor.Service;
using RocketMonitor.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IEventStore, MemoryEventStore>();
builder.Services.AddScoped<IQueryController, QueryController>();
builder.Services.AddScoped<ICommandController, CommandController>();
var streamConfig = new RocketConfiguration();
builder.Configuration.Bind("Rocket", streamConfig);
builder.Services.AddSingleton(streamConfig);

builder.Services.AddControllers().AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    if (File.Exists(xmlFilename)) options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

const string urls = "http://localhost:8089;https://localhost:8088";
builder.WebHost.UseUrls(builder.Configuration.GetSection("urls").Get<string>() ?? urls);
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();