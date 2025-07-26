using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HelloWorldApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with MySQL

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Read JWT secret key from config
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey);

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuerSigningKey = true,
IssuerSigningKey = new SymmetricSecurityKey(key),
ValidateIssuer = false,
ValidateAudience = false
};
});

builder.Services.AddAuthorization();

// Configure Swagger with JWT Bearer support
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new OpenApiInfo
{
Title = "Your API",
Version = "v1",
Description = "API with JWT Authentication"
});

c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme  
{  
    Name = "Authorization",  
    Type = SecuritySchemeType.Http,  
    Scheme = "Bearer",  
    BearerFormat = "JWT",  
    In = ParameterLocation.Header,  
    Description = "Enter 'Bearer' [space] and then your valid JWT token."  
});  

c.AddSecurityRequirement(new OpenApiSecurityRequirement  
{  
    {  
        new OpenApiSecurityScheme  
        {  
            Reference = new OpenApiReference   
            {   
                Type = ReferenceType.SecurityScheme,   
                Id = "Bearer"   
            }  
        },  
        new string[] {}  
    }  
});

});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Enable Swagger middleware in development environment
app.UseSwagger();
app.UseSwaggerUI(c =>
{
c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
c.RoutePrefix = ""; // Or "swagger" if you want it under /swagger
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();