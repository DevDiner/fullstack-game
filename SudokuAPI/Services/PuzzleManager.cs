using SudokuAPI.Models;

namespace SudokuAPI.Services
{
    /// <summary>
    /// Manages CRUD operations for Sudoku puzzles
    /// </summary>
    public class PuzzleManager
    {
        public List<SudokuPuzzle> Puzzles { get; private set; } = new List<SudokuPuzzle>();
        private int nextId = 1;

        // CREATE
        public SudokuPuzzle AddPuzzle(SudokuPuzzle puzzle)
        {
            puzzle.Id = nextId++;
            puzzle.CreatedDate = DateTime.Now;
            Puzzles.Add(puzzle);
            return puzzle;
        }

        // READ
        public SudokuPuzzle? GetPuzzle(int id)
        {
            return Puzzles.FirstOrDefault(p => p.Id == id);
        }

        public List<SudokuPuzzle> GetAllPuzzles()
        {
            return Puzzles.ToList();
        }

        public List<SudokuPuzzle> GetPuzzlesByDifficulty(DifficultyLevel difficulty)
        {
            return Puzzles.Where(p => p.Difficulty == difficulty).ToList();
        }

        // UPDATE
        public bool UpdatePuzzle(int id, SudokuPuzzle updatedPuzzle)
        {
            var puzzle = GetPuzzle(id);
            if (puzzle != null)
            {
                updatedPuzzle.Id = id;
                var index = Puzzles.IndexOf(puzzle);
                Puzzles[index] = updatedPuzzle;
                return true;
            }
            return false;
        }

        public bool RecordPlay(int id, bool completed, int timeSeconds)
        {
            var puzzle = GetPuzzle(id);
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
                return true;
            }
            return false;
        }

        // DELETE
        public bool DeletePuzzle(int id)
        {
            var puzzle = GetPuzzle(id);
            if (puzzle != null)
            {
                Puzzles.Remove(puzzle);
                return true;
            }
            return false;
        }

        // ANALYTICS (LINQ Queries)
        public Dictionary<DifficultyLevel, int> GetPuzzleCountByDifficulty()
        {
            return Puzzles
                .GroupBy(p => p.Difficulty)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public List<SudokuPuzzle> GetMostPlayedPuzzles(int count = 10)
        {
            return Puzzles
                .OrderByDescending(p => p.TimesPlayed)
                .Take(count)
                .ToList();
        }

        public double GetAverageCompletionRate()
        {
            return Puzzles.Any() ? Puzzles.Average(p => p.CompletionRate) : 0;
        }

        public SudokuPuzzle? GetRandomPuzzle(DifficultyLevel? difficulty = null)
        {
            var puzzles = difficulty.HasValue 
                ? Puzzles.Where(p => p.Difficulty == difficulty.Value).ToList()
                : Puzzles;
            
            return puzzles.Any() 
                ? puzzles[new Random().Next(puzzles.Count)] 
                : null;
        }
    }
}
