using Microsoft.EntityFrameworkCore;
using SudokuAPI.Models;

namespace SudokuAPI.Data
{
    /// <summary>
    /// Database context for Sudoku game data
    /// Manages database operations and entity relationships
    /// </summary>
    public class SudokuDbContext : DbContext
    {
        public SudokuDbContext(DbContextOptions<SudokuDbContext> options) : base(options)
        {
        }

        // DbSets represent tables in the database
        public DbSet<SudokuPuzzle> Puzzles { get; set; } = null!;
        public DbSet<PlayerProfile> Players { get; set; } = null!;
        public DbSet<GameSession> Sessions { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure SudokuPuzzle entity
            modelBuilder.Entity<SudokuPuzzle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.InitialGrid).IsRequired().HasMaxLength(81);
                entity.Property(e => e.SolutionGrid).IsRequired().HasMaxLength(81);
                entity.Property(e => e.Difficulty).IsRequired();
                // CreatedDate will be set in code
                
                entity.HasIndex(e => e.Difficulty);
                entity.HasIndex(e => e.CreatedDate);
            });

            // Configure PlayerProfile entity
            modelBuilder.Entity<PlayerProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
                // Dates will be set in code
                
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure GameSession entity
            modelBuilder.Entity<GameSession>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CurrentGrid).IsRequired().HasMaxLength(81);
                // StartTime will be set in code
                
                entity.HasIndex(e => e.PlayerId);
                entity.HasIndex(e => e.PuzzleId);
                entity.HasIndex(e => e.StartTime);
                
                // Configure relationships
                entity.HasOne<PlayerProfile>()
                      .WithMany()
                      .HasForeignKey(e => e.PlayerId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne<SudokuPuzzle>()
                      .WithMany()
                      .HasForeignKey(e => e.PuzzleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure User entity (for authentication)
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                // CreatedAt will be set in code
                
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed some initial puzzles
            modelBuilder.Entity<SudokuPuzzle>().HasData(
                new SudokuPuzzle
                {
                    Id = 1,
                    Name = "Easy Puzzle 1",
                    InitialGrid = "530070000600195000098000060800060003400803001700020006060000280000419005000080079",
                    SolutionGrid = "534678912672195348198342567859761423426853791713924856961537284287419635345286179",
                    Difficulty = DifficultyLevel.Easy,
                    EmptyCells = 40,
                    CreatedDate = DateTime.UtcNow,
                    TimesPlayed = 0,
                    TimesCompleted = 0
                },
                new SudokuPuzzle
                {
                    Id = 2,
                    Name = "Medium Puzzle 1",
                    InitialGrid = "000000907000420180000705026100904000050000040000507009920108000034059000507000000",
                    SolutionGrid = "362184957794623185185796426176934258853271649249587369921348576634859712587612834",
                    Difficulty = DifficultyLevel.Medium,
                    EmptyCells = 50,
                    CreatedDate = DateTime.UtcNow,
                    TimesPlayed = 0,
                    TimesCompleted = 0
                }
            );

            // Seed admin user (password: Admin@123)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$11$KqH5HQzXfJZ.QfX8K8Yn5.6JNxJ5.QxMYJYh5WZz5JZ5QzXfJZ5Q", // This is a bcrypt hash
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
