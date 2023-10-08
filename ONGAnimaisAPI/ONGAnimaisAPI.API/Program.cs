using ONGAnimaisAPI.API.Configurations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Serilog
builder.Host.UseSerilogConfiguration();
builder.Services.AddSerilogConfiguration(builder.Configuration);
//Swagger
builder.Services.AddSwaggerConfiguration(builder.Configuration);
//JWT Autentication
builder.Services.AddAutenticationConfiguration(builder.Configuration);

//Project Dependency Injection
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDBContextConfiguration(builder.Configuration);
builder.Services.AddDependencyInjection();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwaggerConfiguration();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
