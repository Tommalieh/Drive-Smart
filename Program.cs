using System.Text;
using Drivia.Data;
using Drivia.Repositories;
using Drivia.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Define CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://www.drivia.it")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// SwashBuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!))
        };
    });
builder.Services.AddAuthorization();

// Register DB and Services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserRefreshTokenRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TheoryService>();
builder.Services.AddScoped<QuizService>();

try
{
    var app = builder.Build();
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            var errorResponse = new
            {
                Message = "An unexpected error occurred.",
                Detail = exception?.Message
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        });
    });
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseCors("AllowFrontend");
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine($".NET version: {Environment.Version}");
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine("Drivia Exception: " + e);
    Console.WriteLine("-------------------------------------------");
    throw;
}