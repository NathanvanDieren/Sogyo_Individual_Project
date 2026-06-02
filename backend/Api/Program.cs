using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens;               
using System.Text;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers(); 

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();