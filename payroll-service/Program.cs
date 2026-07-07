using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using payroll_service.Data;
using payroll_service.Repositories;
using payroll_service.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Lắng nghe tất cả IP — HTTP 5000 + HTTPS 7300
// Lắng nghe tất cả IP — HTTP 5000 + HTTPS 7300 (đọc từ env nếu có, fallback mặc định)
var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://0.0.0.0:5000;https://0.0.0.0:7300";
builder.WebHost.UseUrls(urls);

// Kết nối MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PayrollDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

// Đăng ký Repository và Service
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<IPayrollService, PayrollService>();

// Đăng ký HR Service Client
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHrServiceClient, HrServiceClient>();

// Đăng ký Attendance Service Client + Payroll Calculator
builder.Services.AddScoped<IAttendanceServiceClient, AttendanceServiceClient>();
builder.Services.AddScoped<IPayrollCalculatorService, PayrollCalculatorService>();

// Đăng ký Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var secret = jwtSection["Secret"] ?? string.Empty;
var issuer = jwtSection["Issuer"] ?? string.Empty;
var audience = jwtSection["Audience"] ?? string.Empty;
var key = Encoding.UTF8.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Payroll Service API",
        Version = "v1.0",
        Description = "Hệ thống quản lý lương nhân viên - Nhóm 3"
    });
});

// CORS — cho phép tất cả origin (frontend kết nối được)
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseCors("AllowAll");

// Tự động migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PayrollDbContext>();
    db.Database.Migrate();
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payroll API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();