using SudokuAPI.Models;

namespace SudokuAPI.Services
{
    /// <summary>
    /// Manages game sessions (individual plays)
    /// </summary>
    public class SessionManager
    {
        public List<GameSession> Sessions { get; private set; } = new List<GameSession>();
        private int nextId = 1;

        // CREATE
        public GameSession StartSession(int playerId, int puzzleId, string initialGrid)
        {
            var session = new GameSession
            {
                Id = nextId++,
                PlayerId = playerId,
                PuzzleId = puzzleId,
                CurrentGrid = initialGrid,
                StartTime = DateTime.Now,
                HintsUsed = 0,
                MistakesMade = 0,
                IsCompleted = false,
                IsAbandoned = false
            };
            Sessions.Add(session);
            return session;
        }

        // READ
        public GameSession? GetSession(int id)
        {
            return Sessions.FirstOrDefault(s => s.Id == id);
        }

        public List<GameSession> GetAllSessions()
        {
            return Sessions.ToList();
        }

        public List<GameSession> GetSessionsByPlayer(int playerId)
        {
            return Sessions.Where(s => s.PlayerId == playerId).ToList();
        }

        public List<GameSession> GetSessionsByPuzzle(int puzzleId)
        {
            return Sessions.Where(s => s.PuzzleId == puzzleId).ToList();
        }

        public GameSession? GetActiveSession(int playerId)
        {
            return Sessions.FirstOrDefault(s => s.PlayerId == playerId && !s.IsCompleted && !s.IsAbandoned);
        }

        // UPDATE
        public bool UpdateSession(int id, string currentGrid, int elapsedSeconds, int hintsUsed, int mistakes)
        {
            var session = GetSession(id);
            if (session != null)
            {
                session.CurrentGrid = currentGrid;
                session.ElapsedSeconds = elapsedSeconds;
                session.HintsUsed = hintsUsed;
                session.MistakesMade = mistakes;
                return true;
            }
            return false;
        }

        public bool CompleteSession(int id)
        {
            var session = GetSession(id);
            if (session != null)
            {
                session.IsCompleted = true;
                session.EndTime = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool AbandonSession(int id)
        {
            var session = GetSession(id);
            if (session != null)
            {
                session.IsAbandoned = true;
                session.EndTime = DateTime.Now;
                return true;
            }
            return false;
        }

        // DELETE
        public bool DeleteSession(int id)
        {
            var session = GetSession(id);
            if (session != null)
            {
                Sessions.Remove(session);
                return true;
            }
            return false;
        }

        // ANALYTICS (LINQ Queries)
        public List<GameSession> GetCompletedSessions()
        {
            return Sessions.Where(s => s.IsCompleted).ToList();
        }

        public List<GameSession> GetFastestCompletions(int count = 10)
        {
            return Sessions
                .Where(s => s.IsCompleted)
                .OrderBy(s => s.ElapsedSeconds)
                .Take(count)
                .ToList();
        }

        public double GetAverageCompletionTime()
        {
            var completed = Sessions.Where(s => s.IsCompleted).ToList();
            return completed.Any() ? completed.Average(s => s.ElapsedSeconds) : 0;
        }

        public double GetCompletionRate()
        {
            return Sessions.Any() 
                ? (double)Sessions.Count(s => s.IsCompleted) / Sessions.Count * 100 
                : 0;
        }

        public int GetTotalHintsUsed()
        {
            return Sessions.Sum(s => s.HintsUsed);
        }

        public double GetAverageHintsPerSession()
        {
            return Sessions.Any() ? Sessions.Average(s => s.HintsUsed) : 0;
        }

        public Dictionary<int, int> GetSessionCountByPlayer()
        {
            return Sessions
                .GroupBy(s => s.PlayerId)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<int, int> GetSessionCountByPuzzle()
        {
            return Sessions
                .GroupBy(s => s.PuzzleId)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
