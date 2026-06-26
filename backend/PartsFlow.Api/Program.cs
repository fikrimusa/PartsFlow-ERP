using System.Net;
using Microsoft.EntityFrameworkCore;
using PartsFlow.Api.Data;
using PartsFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        if (IsEnabled(builder.Configuration["ALLOW_ANY_CORS"]))
        {
            policy.AllowAnyOrigin();
        }
        else
        {
            var frontendUrls = GetFrontendUrls(builder.Configuration);
            policy.WithOrigins(frontendUrls);
        }

        policy.AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var databaseConnectionString = GetDatabaseConnectionString(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(databaseConnectionString));
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (IsEnabled(app.Configuration["APPLY_MIGRATIONS"]))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors("Frontend");

app.MapControllers();

app.Run();

static string[] GetFrontendUrls(IConfiguration configuration)
{
    var configuredUrls = configuration["FRONTEND_URLS"];

    if (string.IsNullOrWhiteSpace(configuredUrls))
    {
        return ["http://localhost:3000"];
    }

    return configuredUrls
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}

static string GetDatabaseConnectionString(IConfiguration configuration)
{
    var databaseUrl = configuration["DATABASE_URL"];

    if (!string.IsNullOrWhiteSpace(databaseUrl))
    {
        return ConvertDatabaseUrlToNpgsqlConnectionString(databaseUrl);
    }

    return configuration.GetConnectionString("PartsFlowDatabase")
        ?? throw new InvalidOperationException("Database connection string is not configured.");
}

static string ConvertDatabaseUrlToNpgsqlConnectionString(string databaseUrl)
{
    if (!databaseUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
        && !databaseUrl.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        return databaseUrl;
    }

    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);
    var username = userInfo.Length > 0 ? WebUtility.UrlDecode(userInfo[0]) : string.Empty;
    var password = userInfo.Length > 1 ? WebUtility.UrlDecode(userInfo[1]) : string.Empty;
    var database = uri.AbsolutePath.TrimStart('/');
    var port = uri.Port > 0 ? uri.Port : 5432;

    return string.Join(';',
        $"Host={uri.Host}",
        $"Port={port}",
        $"Database={database}",
        $"Username={username}",
        $"Password={password}",
        "SSL Mode=Require",
        "Trust Server Certificate=true");
}

static bool IsEnabled(string? value)
{
    return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
        || string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
        || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
}
