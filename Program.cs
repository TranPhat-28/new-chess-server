global using new_chess_server.Models;
global using new_chess_server.Models.Enums;
global using new_chess_server.DTOs.AuthenticationDTO;
global using new_chess_server.Services.Authentication;
global using Microsoft.EntityFrameworkCore;
using new_chess_server.Data;
using new_chess_server.Services.OAuth;

// Enable CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

// The Connection string
var ConnectionString = Environment.GetEnvironmentVariable("DefaultDatabaseConnectionString");
// var ConnectionString = builder.Configuration["Data:DefaultConnection"];

// Add the DbContext
builder.Services.AddDbContext<DataContext>(options
    => options.UseNpgsql(ConnectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IOAuthService, OAuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
