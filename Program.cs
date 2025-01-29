global using new_chess_server.Models;
global using new_chess_server.Models.Enums;
global using new_chess_server.DTOs.AuthenticationDTO;
global using new_chess_server.Services.Authentication;
global using Microsoft.EntityFrameworkCore;
global using System.Security.Claims;
global using Microsoft.AspNetCore.SignalR;
global using AutoMapper;
using new_chess_server.Data;
using new_chess_server.Services.OAuth;
using new_chess_server.Services.QuickPlay;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using new_chess_server.Services.Profile;
using new_chess_server.Services.Social;
using new_chess_server.Services.Friends;
using new_chess_server.Services.Stockfish;
using new_chess_server.Services.PracticeMode;
using new_chess_server.SignalR;

// Enable CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:5173", "https://new-chess-client.onrender.com")
                .AllowCredentials();
        });
});

// -----------CHANGE FOR DEPLOYMENT----------------
// The Connection string
// var ConnectionString = Environment.GetEnvironmentVariable("DefaultDatabaseConnectionString"); // DEPLOY
var ConnectionString = builder.Configuration["Data:DefaultConnection"]; // DEVELOP

// Add the DbContext
builder.Services.AddDbContext<DataContext>(options
    => options.UseNpgsql(ConnectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Dependency Injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IOAuthService, OAuthService>();
builder.Services.AddScoped<IProfileSerivce, ProfileService>();
builder.Services.AddScoped<IQuickPlayHandlerService, QuickPlayHandlerService>();
builder.Services.AddScoped<ISocialService, SocialService>();
builder.Services.AddScoped<IFriendsService, FriendsService>();
builder.Services.AddScoped<IPracticeModeService, PracticeModeService>();
builder.Services.AddSingleton<IStockfishService, StockfishService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<OnlineTracker>();
builder.Services.AddSingleton<GameLobbyTracker>();

// -----------CHANGE FOR DEPLOYMENT----------------
// JWT Secret
var secretToken = builder.Configuration["JWT:Token"]; // DEVELOP
// var secretToken = Environment.GetEnvironmentVariable("JWTSecretString"); // DEPLOY

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretToken!)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };

    // JWT config for Signal R
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MainConnectionHub>("hubs/main");
app.MapHub<GameLobbyHub>("hubs/lobby");

app.Run();
