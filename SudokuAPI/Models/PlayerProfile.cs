namespace SudokuAPI.Models
{
    /// <summary>
    /// Represents a player's profile with stats and preferences
    /// </summary>
    public class PlayerProfile
    {
        public int Id { get; set; }
        public string Username { get; set; } = "Player";
        public string Email { get; set; } = "";
        public int TotalGamesPlayed { get; set; }
        public int TotalGamesCompleted { get; set; }
        public int TotalHintsUsed { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int BestTimeSeconds { get; set; }
        public int TotalPlayTimeSeconds { get; set; }
        public DateTime AccountCreated { get; set; }
        public DateTime LastPlayed { get; set; }
        public DifficultyLevel PreferredDifficulty { get; set; }
        public bool DarkMode { get; set; }

        public double CompletionRate => TotalGamesPlayed > 0 ? (double)TotalGamesCompleted / TotalGamesPlayed * 100 : 0;
        public double AverageHintsPerGame => TotalGamesPlayed > 0 ? (double)TotalHintsUsed / TotalGamesPlayed : 0;

        public override string ToString()
        {
            return $"[{Id}] {Username} | Games: {TotalGamesPlayed} | Completed: {TotalGamesCompleted} | Streak: {CurrentStreak}";
        }

        public string GetDetailedInfo()
        {
            return $@"
╔════════════════════════════════════════╗
║         PLAYER PROFILE                 ║
╠════════════════════════════════════════╣
║ ID: {Id}
║ Username: {Username}
║ Email: {Email}
║ 
║ STATISTICS:
║ Games Played: {TotalGamesPlayed}
║ Games Completed: {TotalGamesCompleted}
║ Completion Rate: {CompletionRate:F1}%
║ Hints Used: {TotalHintsUsed} (avg: {AverageHintsPerGame:F1}/game)
║ 
║ STREAKS:
║ Current Streak: {CurrentStreak} games
║ Longest Streak: {LongestStreak} games
║ 
║ TIME:
║ Best Time: {BestTimeSeconds}s
║ Total Play Time: {TimeSpan.FromSeconds(TotalPlayTimeSeconds)}
║ 
║ ACCOUNT:
║ Created: {AccountCreated:yyyy-MM-dd}
║ Last Played: {LastPlayed:yyyy-MM-dd HH:mm}
║ Preferred Difficulty: {PreferredDifficulty}
╚════════════════════════════════════════╝";
        }
    }
}
