using Microsoft.EntityFrameworkCore;
using SudokuAPI.Data;
using SudokuAPI.Models;

namespace SudokuAPI.Services
{
    /// <summary>
    /// Manages CRUD operations for Sudoku puzzles using Entity Framework
    /// </summary>
    public class PuzzleManager
    {
        private readonly SudokuDbContext _context;

        public PuzzleManager(SudokuDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<SudokuPuzzle> AddPuzzleAsync(SudokuPuzzle puzzle)
        {
            puzzle.CreatedDate = DateTime.UtcNow;
            _context.Puzzles.Add(puzzle);
            await _context.SaveChangesAsync();
            return puzzle;
        }

        // READ
        public async Task<SudokuPuzzle?> GetPuzzleAsync(int id)
        {
            return await _context.Puzzles.FindAsync(id);
        }

        public async Task<List<SudokuPuzzle>> GetAllPuzzlesAsync()
        {
            return await _context.Puzzles.ToListAsync();
        }

        public async Task<List<SudokuPuzzle>> GetPuzzlesByDifficultyAsync(DifficultyLevel difficulty)
        {
            return await _context.Puzzles
                .Where(p => p.Difficulty == difficulty)
                .ToListAsync();
        }

        // UPDATE
        public async Task<bool> UpdatePuzzleAsync(int id, SudokuPuzzle updatedPuzzle)
        {
            var puzzle = await _context.Puzzles.FindAsync(id);
            if (puzzle != null)
            {
                _context.Entry(puzzle).CurrentValues.SetValues(updatedPuzzle);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RecordPlayAsync(int id, bool completed, int timeSeconds)
        {
            var puzzle = await _context.Puzzles.FindAsync(id);
            if (puzzle != null)
            {
                puzzle.TimesPlayed++;
                if (completed)
                {
                    puzzle.TimesCompleted++;
                    if (!puzzle.BestTimeSeconds.HasValue || timeSeconds < puzzle.BestTimeSeconds)
                    {
                        puzzle.BestTimeSeconds = timeSeconds;
                    }
                    // Update average
                    if (puzzle.AverageTimeSeconds.HasValue)
                    {
                        puzzle.AverageTimeSeconds = (puzzle.AverageTimeSeconds * (puzzle.TimesCompleted - 1) + timeSeconds) / puzzle.TimesCompleted;
                    }
                    else
                    {
                        puzzle.AverageTimeSeconds = timeSeconds;
                    }
                }
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // DELETE
        public async Task<bool> DeletePuzzleAsync(int id)
        {
            var puzzle = await _context.Puzzles.FindAsync(id);
            if (puzzle != null)
            {
                _context.Puzzles.Remove(puzzle);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ANALYTICS (LINQ Queries)
        public async Task<Dictionary<DifficultyLevel, int>> GetPuzzleCountByDifficultyAsync()
        {
            return await _context.Puzzles
                .GroupBy(p => p.Difficulty)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<List<SudokuPuzzle>> GetMostPlayedPuzzlesAsync(int count = 10)
        {
            return await _context.Puzzles
                .OrderByDescending(p => p.TimesPlayed)
                .Take(count)
                .ToListAsync();
        }

        public async Task<double> GetAverageCompletionRateAsync()
        {
            var puzzles = await _context.Puzzles.ToListAsync();
            return puzzles.Any() ? puzzles.Average(p => p.CompletionRate) : 0;
        }

        public async Task<SudokuPuzzle?> GetRandomPuzzleAsync(DifficultyLevel? difficulty = null)
        {
            var query = difficulty.HasValue 
                ? _context.Puzzles.Where(p => p.Difficulty == difficulty.Value)
                : _context.Puzzles;
            
            var puzzles = await query.ToListAsync();
            return puzzles.Any() 
                ? puzzles[new Random().Next(puzzles.Count)] 
                : null;
        }
    }
}
