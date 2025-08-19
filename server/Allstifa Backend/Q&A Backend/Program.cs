using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using Q_A_Backend.Data;
using Q_A_Backend.Mapper;
using Q_A_Backend.Repositories.Implementations;
using Q_A_Backend.Repositories.Interfaces;
using Q_A_Backend.Services.Implementations;
using Q_A_Backend.Services.Interfaces;
using QnABackend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ===========================================
// 🔐 JWT Configuration
// ===========================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];
var key = jwtSettings["Key"];

// ===========================================
// 🚀 Services Registration
// ===========================================
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); // FIX: Registers the IHttpContextAccessor service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
    };
});

// ===========================================
// 🧩 Dependency Injection
// ===========================================
builder.Services.AddScoped<DapperDbContext>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IFileService, FileService>();

// Repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// ===========================================
// ⚠️ Global Error Middleware
// ===========================================
app.UseMiddleware<Q_A_Backend.Middleware.ExceptionHandlingMiddleware>();

// ===========================================
// 📂 Static File Middleware for /uploads
// ===========================================
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(); // For wwwroot by default
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads",
    ServeUnknownFileTypes = true
});

// ===========================================
// 🌐 Swagger & HTTP Config
// ===========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();