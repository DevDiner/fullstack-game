using SudokuAPI.Models;

namespace SudokuAPI.Services
{
    /// <summary>
    /// Manages CRUD operations for player profiles
    /// </summary>
    public class PlayerManager
    {
        public List<PlayerProfile> Players { get; private set; } = new List<PlayerProfile>();
        private int nextId = 1;

        // CREATE
        public PlayerProfile AddPlayer(PlayerProfile player)
        {
            player.Id = nextId++;
            player.AccountCreated = DateTime.Now;
            player.LastPlayed = DateTime.Now;
            Players.Add(player);
            return player;
        }

        // READ
        public PlayerProfile? GetPlayer(int id)
        {
            return Players.FirstOrDefault(p => p.Id == id);
        }

        public PlayerProfile? GetPlayerByUsername(string username)
        {
            return Players.FirstOrDefault(p => p.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public List<PlayerProfile> GetAllPlayers()
        {
            return Players.ToList();
        }

        // UPDATE
        public bool UpdatePlayer(int id, PlayerProfile updatedPlayer)
        {
            var player = GetPlayer(id);
            if (player != null)
            {
                updatedPlayer.Id = id;
                updatedPlayer.AccountCreated = player.AccountCreated; // Preserve creation date
                var index = Players.IndexOf(player);
                Players[index] = updatedPlayer;
                return true;
            }
            return false;
        }

        public bool RecordGamePlayed(int playerId, bool completed, int timeSeconds, int hintsUsed)
        {
            var player = GetPlayer(playerId);
            if (player != null)
            {
                player.TotalGamesPlayed++;
                player.TotalHintsUsed += hintsUsed;
                player.TotalPlayTimeSeconds += timeSeconds;
                player.LastPlayed = DateTime.Now;

                if (completed)
                {
                    player.TotalGamesCompleted++;
                    player.CurrentStreak++;
                    if (player.CurrentStreak > player.LongestStreak)
                    {
                        player.LongestStreak = player.CurrentStreak;
                    }
player.BestTimeSeconds = (player.BestTimeSeconds == 0 || timeSeconds < player.BestTimeSeconds)
                        ? timeSeconds
                        : player.BestTimeSeconds;
                }
                else
                {
                    player.CurrentStreak = 0;
                }
                return true;
            }
            return false;
        }

        // DELETE
        public bool DeletePlayer(int id)
        {
            var player = GetPlayer(id);
            if (player != null)
            {
                Players.Remove(player);
                return true;
            }
            return false;
        }

        // ANALYTICS (LINQ Queries)
        public List<PlayerProfile> GetLeaderboard(int count = 10)
        {
            return Players
                .OrderByDescending(p => p.TotalGamesCompleted)
                .ThenByDescending(p => p.CompletionRate)
                .Take(count)
                .ToList();
        }

        public List<PlayerProfile> GetTopPlayersByStreak(int count = 10)
        {
            return Players
                .OrderByDescending(p => p.CurrentStreak)
                .Take(count)
                .ToList();
        }

        public List<PlayerProfile> GetFastestPlayers(int count = 10)
        {
            return Players
                .Where(p => p.BestTimeSeconds > 0)
                .OrderBy(p => p.BestTimeSeconds)
                .Take(count)
                .ToList();
        }

        public double GetAverageCompletionRate()
        {
            return Players.Any() ? Players.Average(p => p.CompletionRate) : 0;
        }

        public int GetTotalGamesPlayed()
        {
            return Players.Sum(p => p.TotalGamesPlayed);
        }

        public int GetActivePlayers(int daysActive = 7)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysActive);
            return Players.Count(p => p.LastPlayed >= cutoffDate);
        }
    }
}
