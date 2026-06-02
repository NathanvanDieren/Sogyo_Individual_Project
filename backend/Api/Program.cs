using Persistence;
using Domain.Services;
using Api.Middleware;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers(); 

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Services.AddScoped<CurrentUserService>();
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<UserLoaderMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();