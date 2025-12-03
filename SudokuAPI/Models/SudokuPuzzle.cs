namespace SudokuAPI.Models
{
    /// <summary>
    /// Represents a Sudoku puzzle with its grid, solution, and metadata
    /// </summary>
    public class SudokuPuzzle
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Untitled Puzzle";
        public string InitialGrid { get; set; } = ""; // 81-character string
        public string SolutionGrid { get; set; } = ""; // 81-character string
        public DifficultyLevel Difficulty { get; set; }
        public int EmptyCells { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TimesPlayed { get; set; }
        public int TimesCompleted { get; set; }
        public int? BestTimeSeconds { get; set; }
        public double? AverageTimeSeconds { get; set; }

        public double CompletionRate => TimesPlayed > 0 ? (double)TimesCompleted / TimesPlayed * 100 : 0;

        public override string ToString()
        {
            return $"[{Id}] {Name} ({Difficulty}) - {EmptyCells} empty cells | Played: {TimesPlayed} | Completion: {CompletionRate:F1}%";
        }

        public string GetDetailedInfo()
        {
            return $@"
╔════════════════════════════════════════╗
║          PUZZLE DETAILS                ║
╠════════════════════════════════════════╣
║ ID: {Id}
║ Name: {Name}
║ Difficulty: {Difficulty}
║ Empty Cells: {EmptyCells}
║ Times Played: {TimesPlayed}
║ Times Completed: {TimesCompleted}
║ Completion Rate: {CompletionRate:F1}%
║ Best Time: {(BestTimeSeconds.HasValue ? $"{BestTimeSeconds}s" : "N/A")}
║ Average Time: {(AverageTimeSeconds.HasValue ? $"{AverageTimeSeconds:F1}s" : "N/A")}
║ Created: {CreatedDate:yyyy-MM-dd}
╚════════════════════════════════════════╝";
        }
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard,
        Expert,
        Evil
    }
}
