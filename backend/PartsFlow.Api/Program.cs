using Microsoft.EntityFrameworkCore;
using PartsFlow.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PartsFlowDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PartsFlowDatabase")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
