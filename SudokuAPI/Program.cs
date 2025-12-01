using SudokuAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register our services as singletons (in-memory data)
builder.Services.AddSingleton<PuzzleManager>();
builder.Services.AddSingleton<PlayerManager>();
builder.Services.AddSingleton<SessionManager>();

// Add CORS to allow calls from the Sudoku frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSudokuFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSudokuFrontend");
app.UseAuthorization();
app.MapControllers();

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
║              🎮 API Backend v2.0 🎮                      ║
║                                                          ║
╚══════════════════════════════════════════════════════════╝
");

Console.WriteLine("✅ Sudoku API is running!");
Console.WriteLine($"📍 API URL: {app.Urls.FirstOrDefault() ?? "http://localhost:5000"}");
Console.WriteLine("📚 Swagger UI: /swagger");
Console.WriteLine("🎯 Ready to serve your Sudoku game!\n");

app.Run();
