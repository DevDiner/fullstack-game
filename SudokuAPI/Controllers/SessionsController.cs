using Microsoft.AspNetCore.Mvc;
using SudokuAPI.Models;
using SudokuAPI.Services;

namespace SudokuAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly SessionManager _sessionManager;

        public SessionsController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        // GET: api/sessions
        [HttpGet]
        public async Task<ActionResult<List<GameSession>>> GetAll()
        {
            var sessions = await _sessionManager.GetAllSessionsAsync();
            return Ok(sessions);
        }

        // GET: api/sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameSession>> GetById(int id)
        {
            var session = await _sessionManager.GetSessionAsync(id);
            if (session == null)
                return NotFound(new { message = $"Session with ID {id} not found" });
            
            return Ok(session);
        }

        // GET: api/sessions/player/5
        [HttpGet("player/{playerId}")]
        public async Task<ActionResult<List<GameSession>>> GetByPlayer(int playerId)
        {
            var sessions = await _sessionManager.GetSessionsByPlayerAsync(playerId);
            return Ok(sessions);
        }

        // GET: api/sessions/puzzle/5
        [HttpGet("puzzle/{puzzleId}")]
        public async Task<ActionResult<List<GameSession>>> GetByPuzzle(int puzzleId)
        {
            var sessions = await _sessionManager.GetSessionsByPuzzleAsync(puzzleId);
            return Ok(sessions);
        }

        // GET: api/sessions/player/5/active
        [HttpGet("player/{playerId}/active")]
        public async Task<ActionResult<GameSession>> GetActiveSession(int playerId)
        {
            var session = await _sessionManager.GetActiveSessionAsync(playerId);
            if (session == null)
                return NotFound(new { message = "No active session found" });
            
            return Ok(session);
        }

        // POST: api/sessions
        [HttpPost]
        public async Task<ActionResult<GameSession>> Start([FromBody] StartSessionRequest request)
        {
            var session = await _sessionManager.StartSessionAsync(request.PlayerId, request.PuzzleId, request.InitialGrid);
            return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
        }

        // PUT: api/sessions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSessionRequest request)
        {
            var updated = await _sessionManager.UpdateSessionAsync(id, request.CurrentGrid, request.ElapsedSeconds, request.HintsUsed, request.Mistakes);
            if (updated)
                return NoContent();
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // POST: api/sessions/5/complete
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var completed = await _sessionManager.CompleteSessionAsync(id);
            if (completed)
                return Ok(new { message = "Session completed successfully" });
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // POST: api/sessions/5/abandon
        [HttpPost("{id}/abandon")]
        public async Task<IActionResult> Abandon(int id)
        {
            var abandoned = await _sessionManager.AbandonSessionAsync(id);
            if (abandoned)
                return Ok(new { message = "Session abandoned" });
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // DELETE: api/sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _sessionManager.DeleteSessionAsync(id);
            if (deleted)
                return NoContent();
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // GET: api/sessions/completed
        [HttpGet("completed")]
        public async Task<ActionResult<List<GameSession>>> GetCompleted()
        {
            var sessions = await _sessionManager.GetCompletedSessionsAsync();
            return Ok(sessions);
        }

        // GET: api/sessions/fastest?count=10
        [HttpGet("fastest")]
        public async Task<ActionResult<List<GameSession>>> GetFastest([FromQuery] int count = 10)
        {
            var sessions = await _sessionManager.GetFastestCompletionsAsync(count);
            return Ok(sessions);
        }

        // GET: api/sessions/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStats()
        {
            var totalSessions = (await _sessionManager.GetAllSessionsAsync()).Count;
            var completionRate = await _sessionManager.GetCompletionRateAsync();
            var avgCompletionTime = await _sessionManager.GetAverageCompletionTimeAsync();
            var totalHints = await _sessionManager.GetTotalHintsUsedAsync();
            var avgHints = await _sessionManager.GetAverageHintsPerSessionAsync();
            
            return Ok(new 
            { 
                totalSessions,
                completionRate,
                averageCompletionTime = avgCompletionTime,
                totalHintsUsed = totalHints,
                averageHintsPerSession = avgHints
            });
        }
    }

    public class StartSessionRequest
    {
        public int PlayerId { get; set; }
        public int PuzzleId { get; set; }
        public string InitialGrid { get; set; } = "";
    }

    public class UpdateSessionRequest
    {
        public string CurrentGrid { get; set; } = "";
        public int ElapsedSeconds { get; set; }
        public int HintsUsed { get; set; }
        public int Mistakes { get; set; }
    }
}
