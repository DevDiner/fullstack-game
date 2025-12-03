using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using SudokuAPI.Data;
using SudokuAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== DATABASE CONFIGURATION =====
builder.Services.AddDbContext<SudokuDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== AUTHENTICATION & AUTHORIZATION =====
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured"));

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
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("PlayerOrAdmin", policy => policy.RequireRole("Player", "Admin"));
});

// ===== SERVICES REGISTRATION =====
// Register custom services
builder.Services.AddScoped<AuthService>();

// Register managers (note: changed from Singleton to Scoped for DB context compatibility)
builder.Services.AddScoped<PuzzleManager>();
builder.Services.AddScoped<PlayerManager>();
builder.Services.AddScoped<SessionManager>();

// ===== API CONFIGURATION =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keep PascalCase
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

// ===== SWAGGER CONFIGURATION WITH JWT =====
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sudoku Game API",
        Version = "v2.0",
        Description = "RESTful API for Sudoku game management with JWT authentication, Entity Framework Core, and SQL Server database",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com"
        }
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});

// ===== CORS CONFIGURATION =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// ===== DATABASE INITIALIZATION =====
// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SudokuDbContext>();
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("✅ Database initialized successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error initializing database: {ex.Message}");
    }
}

// ===== HTTP REQUEST PIPELINE =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sudoku API V2");
        c.DefaultModelsExpandDepth(-1); // Collapse models by default
    });
}

app.UseCors("AllowAll");
app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();
app.MapControllers();

// ===== STARTUP BANNER =====
Console.WriteLine(@"
╔══════════════════════════════════════════════════════════╗
║                                                          ║
║     ███████╗██╗   ██╗██████╗  ██████╗ ██╗  ██╗██╗   ██╗ ║
║     ██╔════╝██║   ██║██╔══██╗██╔═══██╗██║ ██╔╝██║   ██║ ║
║     ███████╗██║   ██║██║  ██║██║   ██║█████╔╝ ██║   ██║ ║
║     ╚════██║██║   ██║██║  ██║██║   ██║██╔═██╗ ██║   ██║ ║
║     ███████║╚██████╔╝██████╔╝╚██████╔╝██║  ██╗╚██████╔╝ ║
║     ╚══════╝ ╚═════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝ ╚═════╝  ║
║                                                          ║
║          🎮 GAME API v2.0 - PRODUCTION READY 🎮          ║
║                                                          ║
╚══════════════════════════════════════════════════════════╝
");

Console.WriteLine("✅ Sudoku API with Database & JWT Auth is running!");
Console.WriteLine($"📍 API URL: {app.Urls.FirstOrDefault() ?? "http://localhost:5000"}");
Console.WriteLine("📚 Swagger UI: /swagger");
Console.WriteLine("🔐 JWT Authentication: Enabled");
Console.WriteLine("💾 Database: SQLite (Entity Framework Core)");
Console.WriteLine("🎯 Ready for production!\n");

app.Run();
