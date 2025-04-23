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

using UserDao = API.DAOAPI.UserDao;
using TopicDAO = API.DAOAPI.TopicDAO;
using LessonDAO = API.DAOAPI.LessonDAO;
using Question_Dao = API.DAOAPI.Question_Dao;
using UserBadges_Dao = API.DAOAPI.UserBadges_Dao;
using UserProgress_Dao = API.DAOAPI.UserProgress_Dao;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext và chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  // DefaultConnection là tên chuỗi kết nối trong appsettings.json
builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IUserService, UserService>();
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
// Cấu hình đọc SecretKey từ user-secrets
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]; // Đọc từ user-secrets

// Thêm JWT Authentication
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

        // Key dùng để validate signature của JWT
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


// Cấu hình Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Thêm thông tin về JWT Bearer Authentication cho Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token JWT vào đây, ví dụ: Bearer {your_token}"
    });

    // Thêm yêu cầu xác thực Bearer cho các API cần bảo vệ
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
            new string[] {}
        }
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication(); // Thêm Middleware cho Authentication
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
