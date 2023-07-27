using Microservice.JWT;
using Microservice.Models;
using Microservice.Redis;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Serilog;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<redisContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnect"));
});
builder.Services.AddStackExchangeRedisCache(redisOption =>
{
    string connection = builder.Configuration["Redis:redisString"];
    Console.WriteLine(connection);
    redisOption.Configuration = connection;
});
Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddScoped<JsonWebToken>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            // kiểm tra người phát hành issuer có hơp lệ hay không . Nếu đặt là true thì người phát hành hợp lệ
            ValidateAudience = true,
            // kiểm tra người nhận có hợp lệ hay không.
            ValidateLifetime = true,
            // Kiểm tra xem token có còn hợp lệ dựa trên thời gian sống của nó không. Nếu đặt là true, token phải còn thời gian sống.
            ValidateIssuerSigningKey = true,
            // Kiểm tra xem khóa ký của người phát hành token có hợp lệ không. Nếu đặt là true, khóa ký phải hợp lệ.
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero
            //  Thời gian dự phòng cho token hết hạn. Nếu đặt là TimeSpan.Zero, token sẽ hết hạn ngay lập tức sau khi thời gian hết hạn được chỉ định. 
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllRolesPolicy", policy =>
    {
        policy.RequireRole("Admin", "User");
    });
});
builder.Services.AddScoped<IRedisDatabase, RedisDatabase>();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();



app.UseAuthorization();



app.MapControllers();

app.Run();
