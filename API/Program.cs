using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Model;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.IServices;
using Model.DAO;
using API.DAOAPI;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OpenApi.Models;

// Alias các DAO cho rõ ràng
using UserDao = API.DAOAPI.UserDao;
using TopicDAO = API.DAOAPI.TopicDAO;
using LessonDAO = API.DAOAPI.LessonDAO;
using Question_Dao = API.DAOAPI.Question_Dao;
using UserBadges_Dao = API.DAOAPI.UserBadges_Dao;
using UserProgress_Dao = API.DAOAPI.UserProgress_Dao;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICE CONFIG --------------------
// Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký các Service
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});
builder.Services.AddScoped<UserDao>();
builder.Services.AddScoped<TopicDAO>();
builder.Services.AddScoped<Question_Dao>();
builder.Services.AddScoped<LessonDAO>();
builder.Services.AddScoped<UserBadges_Dao>();
builder.Services.AddScoped<UserProgress_Dao>();

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

// JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// CORS ➤ Cho phép tất cả domain gọi đến API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token JWT vào đây, ví dụ: Bearer {your_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Controllers & Swagger Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// -------------------- APP CONFIG --------------------
var app = builder.Build();

// Dùng HTTPS
app.UseHttpsRedirection();

// Bật Swagger UI khi ở chế độ phát triển
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Bật CORS trước khi xác thực
app.UseCors("AllowAll");

// Xác thực + phân quyền
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Chạy app
app.Run();
