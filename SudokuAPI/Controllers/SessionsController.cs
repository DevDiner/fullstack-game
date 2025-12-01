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
        public ActionResult<List<GameSession>> GetAll()
        {
            return Ok(_sessionManager.GetAllSessions());
        }

        // GET: api/sessions/5
        [HttpGet("{id}")]
        public ActionResult<GameSession> GetById(int id)
        {
            var session = _sessionManager.GetSession(id);
            if (session == null)
                return NotFound(new { message = $"Session with ID {id} not found" });
            
            return Ok(session);
        }

        // GET: api/sessions/player/5
        [HttpGet("player/{playerId}")]
        public ActionResult<List<GameSession>> GetByPlayer(int playerId)
        {
            return Ok(_sessionManager.GetSessionsByPlayer(playerId));
        }

        // GET: api/sessions/puzzle/5
        [HttpGet("puzzle/{puzzleId}")]
        public ActionResult<List<GameSession>> GetByPuzzle(int puzzleId)
        {
            return Ok(_sessionManager.GetSessionsByPuzzle(puzzleId));
        }

        // GET: api/sessions/player/5/active
        [HttpGet("player/{playerId}/active")]
        public ActionResult<GameSession> GetActiveSession(int playerId)
        {
            var session = _sessionManager.GetActiveSession(playerId);
            if (session == null)
                return NotFound(new { message = "No active session found" });
            
            return Ok(session);
        }

        // POST: api/sessions
        [HttpPost]
        public ActionResult<GameSession> Start([FromBody] StartSessionRequest request)
        {
            var session = _sessionManager.StartSession(request.PlayerId, request.PuzzleId, request.InitialGrid);
            return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
        }

        // PUT: api/sessions/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateSessionRequest request)
        {
            if (_sessionManager.UpdateSession(id, request.CurrentGrid, request.ElapsedSeconds, request.HintsUsed, request.Mistakes))
                return NoContent();
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // POST: api/sessions/5/complete
        [HttpPost("{id}/complete")]
        public IActionResult Complete(int id)
        {
            if (_sessionManager.CompleteSession(id))
                return Ok(new { message = "Session completed successfully" });
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // POST: api/sessions/5/abandon
        [HttpPost("{id}/abandon")]
        public IActionResult Abandon(int id)
        {
            if (_sessionManager.AbandonSession(id))
                return Ok(new { message = "Session abandoned" });
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // DELETE: api/sessions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_sessionManager.DeleteSession(id))
                return NoContent();
            
            return NotFound(new { message = $"Session with ID {id} not found" });
        }

        // GET: api/sessions/completed
        [HttpGet("completed")]
        public ActionResult<List<GameSession>> GetCompleted()
        {
            return Ok(_sessionManager.GetCompletedSessions());
        }

        // GET: api/sessions/fastest?count=10
        [HttpGet("fastest")]
        public ActionResult<List<GameSession>> GetFastest([FromQuery] int count = 10)
        {
            return Ok(_sessionManager.GetFastestCompletions(count));
        }

        // GET: api/sessions/stats
        [HttpGet("stats")]
        public ActionResult<object> GetStats()
        {
            return Ok(new 
            { 
                totalSessions = _sessionManager.GetAllSessions().Count,
                completionRate = _sessionManager.GetCompletionRate(),
                averageCompletionTime = _sessionManager.GetAverageCompletionTime(),
                totalHintsUsed = _sessionManager.GetTotalHintsUsed(),
                averageHintsPerSession = _sessionManager.GetAverageHintsPerSession()
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
