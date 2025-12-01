namespace SudokuAPI.Models
{
    /// <summary>
    /// Represents an individual game session
    /// </summary>
    public class GameSession
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int PuzzleId { get; set; }
        public string CurrentGrid { get; set; } = ""; // 81-character string showing current state
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ElapsedSeconds { get; set; }
        public int HintsUsed { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsAbandoned { get; set; }
        public int MistakesMade { get; set; }

        public string Status => IsCompleted ? "Completed" : IsAbandoned ? "Abandoned" : "In Progress";

        public override string ToString()
        {
            return $"[{Id}] Player {PlayerId} - Puzzle {PuzzleId} | {Status} | Time: {ElapsedSeconds}s | Hints: {HintsUsed}";
        }

        public string GetDetailedInfo()
        {
            return $@"
╔════════════════════════════════════════╗
║         GAME SESSION                   ║
╠════════════════════════════════════════╣
║ Session ID: {Id}
║ Player ID: {PlayerId}
║ Puzzle ID: {PuzzleId}
║ 
║ STATUS: {Status}
║ 
║ TIME:
║ Started: {StartTime:yyyy-MM-dd HH:mm:ss}
║ {(EndTime.HasValue ? $"Ended: {EndTime:yyyy-MM-dd HH:mm:ss}" : "Still in progress")}
║ Elapsed: {ElapsedSeconds}s ({TimeSpan.FromSeconds(ElapsedSeconds)})
║ 
║ GAMEPLAY:
║ Hints Used: {HintsUsed}
║ Mistakes: {MistakesMade}
╚════════════════════════════════════════╝";
        }
    }
}
