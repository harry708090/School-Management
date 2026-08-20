using Microsoft.EntityFrameworkCore;
using Npgsql;
using SchoolManagement.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var renderDatabaseUrl = builder.Configuration["DATABASE_URL"];
var connectionString = string.IsNullOrWhiteSpace(renderDatabaseUrl)
    ? builder.Configuration.GetConnectionString("DefaultConnection")
    : BuildPostgresConnectionString(renderDatabaseUrl);

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure DATABASE_URL for Render PostgreSQL or ConnectionStrings__DefaultConnection for SQL Server.");
}

builder.Services.AddDbContext<SchoolDbContext>(options =>
{
    if (!string.IsNullOrWhiteSpace(renderDatabaseUrl))
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<SchoolDbContext>().Database;
    database.EnsureCreated();
}


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", () => Results.Ok(new
{
    name = "School Management API",
    endpoints = new[] { "/api/students", "/api/classes", "/api/subjects", "/swagger" }
}));

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

static string BuildPostgresConnectionString(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);

    if (userInfo.Length != 2 || string.IsNullOrWhiteSpace(uri.Host) || string.IsNullOrWhiteSpace(uri.AbsolutePath))
    {
        throw new InvalidOperationException("DATABASE_URL is not a valid PostgreSQL connection URL.");
    }

    var connectionString = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port > 0 ? uri.Port : 5432,
        Database = uri.AbsolutePath.TrimStart('/'),
        Username = Uri.UnescapeDataString(userInfo[0]),
        Password = Uri.UnescapeDataString(userInfo[1]),
        SslMode = SslMode.Require
    };

    return connectionString.ConnectionString;
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
