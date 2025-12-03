using Microsoft.EntityFrameworkCore;
using SudokuAPI.Data;
using SudokuAPI.Models;

namespace SudokuAPI.Services
{
    /// <summary>
    /// Manages CRUD operations for player profiles using Entity Framework
    /// </summary>
    public class PlayerManager
    {
        private readonly SudokuDbContext _context;

        public PlayerManager(SudokuDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<PlayerProfile> AddPlayerAsync(PlayerProfile player)
        {
            player.AccountCreated = DateTime.UtcNow;
            player.LastPlayed = DateTime.UtcNow;
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return player;
        }

        // READ
        public async Task<PlayerProfile?> GetPlayerAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }

        public async Task<PlayerProfile?> GetPlayerByUsernameAsync(string username)
        {
            return await _context.Players
                .FirstOrDefaultAsync(p => p.Username == username);
        }

        public async Task<List<PlayerProfile>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        // UPDATE
        public async Task<bool> UpdatePlayerAsync(int id, PlayerProfile updatedPlayer)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                updatedPlayer.AccountCreated = player.AccountCreated; // Preserve
                _context.Entry(player).CurrentValues.SetValues(updatedPlayer);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RecordGamePlayedAsync(int playerId, bool completed, int timeSeconds, int hintsUsed)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player != null)
            {
                player.TotalGamesPlayed++;
                player.TotalHintsUsed += hintsUsed;
                player.TotalPlayTimeSeconds += timeSeconds;
                player.LastPlayed = DateTime.UtcNow;

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
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // DELETE
        public async Task<bool> DeletePlayerAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ANALYTICS (LINQ Queries)
        public async Task<List<PlayerProfile>> GetLeaderboardAsync(int count = 10)
        {
            return await _context.Players
                .OrderByDescending(p => p.TotalGamesCompleted)
                .ThenByDescending(p => p.CompletionRate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<PlayerProfile>> GetTopPlayersByStreakAsync(int count = 10)
        {
            return await _context.Players
                .OrderByDescending(p => p.CurrentStreak)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<PlayerProfile>> GetFastestPlayersAsync(int count = 10)
        {
            return await _context.Players
                .Where(p => p.BestTimeSeconds > 0)
                .OrderBy(p => p.BestTimeSeconds)
                .Take(count)
                .ToListAsync();
        }

        public async Task<double> GetAverageCompletionRateAsync()
        {
            var players = await _context.Players.ToListAsync();
            return players.Any() ? players.Average(p => p.CompletionRate) : 0;
        }

        public async Task<int> GetTotalGamesPlayedAsync()
        {
            return await _context.Players.SumAsync(p => p.TotalGamesPlayed);
        }

        public async Task<int> GetActivePlayersAsync(int daysActive = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysActive);
            return await _context.Players
                .CountAsync(p => p.LastPlayed >= cutoffDate);
        }
    }
}
