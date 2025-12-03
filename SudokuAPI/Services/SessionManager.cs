using Microsoft.EntityFrameworkCore;
using SudokuAPI.Data;
using SudokuAPI.Models;

namespace SudokuAPI.Services
{
    /// <summary>
    /// Manages game sessions using Entity Framework
    /// </summary>
    public class SessionManager
    {
        private readonly SudokuDbContext _context;

        public SessionManager(SudokuDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<GameSession> StartSessionAsync(int playerId, int puzzleId, string initialGrid)
        {
            var session = new GameSession
            {
                PlayerId = playerId,
                PuzzleId = puzzleId,
                CurrentGrid = initialGrid,
                StartTime = DateTime.UtcNow,
                HintsUsed = 0,
                MistakesMade = 0,
                IsCompleted = false,
                IsAbandoned = false
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        // READ
        public async Task<GameSession?> GetSessionAsync(int id)
        {
            return await _context.Sessions.FindAsync(id);
        }

        public async Task<List<GameSession>> GetAllSessionsAsync()
        {
            return await _context.Sessions.ToListAsync();
        }

        public async Task<List<GameSession>> GetSessionsByPlayerAsync(int playerId)
        {
            return await _context.Sessions
                .Where(s => s.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task<List<GameSession>> GetSessionsByPuzzleAsync(int puzzleId)
        {
            return await _context.Sessions
                .Where(s => s.PuzzleId == puzzleId)
                .ToListAsync();
        }

        public async Task<GameSession?> GetActiveSessionAsync(int playerId)
        {
            return await _context.Sessions
                .FirstOrDefaultAsync(s => s.PlayerId == playerId && !s.IsCompleted && !s.IsAbandoned);
        }

        // UPDATE
        public async Task<bool> UpdateSessionAsync(int id, string currentGrid, int elapsedSeconds, int hintsUsed, int mistakes)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                session.CurrentGrid = currentGrid;
                session.ElapsedSeconds = elapsedSeconds;
                session.HintsUsed = hintsUsed;
                session.MistakesMade = mistakes;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CompleteSessionAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                session.IsCompleted = true;
                session.EndTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AbandonSessionAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                session.IsAbandoned = true;
                session.EndTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // DELETE
        public async Task<bool> DeleteSessionAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ANALYTICS (LINQ Queries)
        public async Task<List<GameSession>> GetCompletedSessionsAsync()
        {
            return await _context.Sessions
                .Where(s => s.IsCompleted)
                .ToListAsync();
        }

        public async Task<List<GameSession>> GetFastestCompletionsAsync(int count = 10)
        {
            return await _context.Sessions
                .Where(s => s.IsCompleted)
                .OrderBy(s => s.ElapsedSeconds)
                .Take(count)
                .ToListAsync();
        }

        public async Task<double> GetAverageCompletionTimeAsync()
        {
            var completed = await _context.Sessions
                .Where(s => s.IsCompleted)
                .ToListAsync();
            return completed.Any() ? completed.Average(s => s.ElapsedSeconds) : 0;
        }

        public async Task<double> GetCompletionRateAsync()
        {
            var total = await _context.Sessions.CountAsync();
            if (total == 0) return 0;
            
            var completed = await _context.Sessions.CountAsync(s => s.IsCompleted);
            return (double)completed / total * 100;
        }

        public async Task<int> GetTotalHintsUsedAsync()
        {
            return await _context.Sessions.SumAsync(s => s.HintsUsed);
        }

        public async Task<double> GetAverageHintsPerSessionAsync()
        {
            var sessions = await _context.Sessions.ToListAsync();
            return sessions.Any() ? sessions.Average(s => s.HintsUsed) : 0;
        }

        public async Task<Dictionary<int, int>> GetSessionCountByPlayerAsync()
        {
            return await _context.Sessions
                .GroupBy(s => s.PlayerId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetSessionCountByPuzzleAsync()
        {
            return await _context.Sessions
                .GroupBy(s => s.PuzzleId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
}
