using Microsoft.EntityFrameworkCore;
using Persistence;
using Api.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers(); 

string connectionString = "Host=localhost;Database=ip_tastebuds;Username=IP_db_owner;Password=wachtwoord123!;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();