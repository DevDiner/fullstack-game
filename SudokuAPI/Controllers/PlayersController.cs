using Microsoft.AspNetCore.Mvc;
using SudokuAPI.Models;
using SudokuAPI.Services;

namespace SudokuAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerManager _playerManager;

        public PlayersController(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        // GET: api/players
        [HttpGet]
        public ActionResult<List<PlayerProfile>> GetAll()
        {
            return Ok(_playerManager.GetAllPlayers());
        }

        // GET: api/players/5
        [HttpGet("{id}")]
        public ActionResult<PlayerProfile> GetById(int id)
        {
            var player = _playerManager.GetPlayer(id);
            if (player == null)
                return NotFound(new { message = $"Player with ID {id} not found" });
            
            return Ok(player);
        }

        // GET: api/players/username/john
        [HttpGet("username/{username}")]
        public ActionResult<PlayerProfile> GetByUsername(string username)
        {
            var player = _playerManager.GetPlayerByUsername(username);
            if (player == null)
                return NotFound(new { message = $"Player '{username}' not found" });
            
            return Ok(player);
        }

        // POST: api/players
        [HttpPost]
        public ActionResult<PlayerProfile> Create([FromBody] PlayerProfile player)
        {
            var created = _playerManager.AddPlayer(player);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/players/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PlayerProfile player)
        {
            if (_playerManager.UpdatePlayer(id, player))
                return NoContent();
            
            return NotFound(new { message = $"Player with ID {id} not found" });
        }

        // POST: api/players/5/game
        [HttpPost("{id}/game")]
        public IActionResult RecordGame(int id, [FromBody] GameRecord record)
        {
            if (_playerManager.RecordGamePlayed(id, record.Completed, record.TimeSeconds, record.HintsUsed))
                return Ok(new { message = "Game recorded successfully" });
            
            return NotFound(new { message = $"Player with ID {id} not found" });
        }

        // DELETE: api/players/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_playerManager.DeletePlayer(id))
                return NoContent();
            
            return NotFound(new { message = $"Player with ID {id} not found" });
        }

        // GET: api/players/leaderboard?count=10
        [HttpGet("leaderboard")]
        public ActionResult<List<PlayerProfile>> GetLeaderboard([FromQuery] int count = 10)
        {
            return Ok(_playerManager.GetLeaderboard(count));
        }

        // GET: api/players/streaks?count=10
        [HttpGet("streaks")]
        public ActionResult<List<PlayerProfile>> GetTopStreaks([FromQuery] int count = 10)
        {
            return Ok(_playerManager.GetTopPlayersByStreak(count));
        }

        // GET: api/players/fastest?count=10
        [HttpGet("fastest")]
        public ActionResult<List<PlayerProfile>> GetFastest([FromQuery] int count = 10)
        {
            return Ok(_playerManager.GetFastestPlayers(count));
        }

        // GET: api/players/stats
        [HttpGet("stats")]
        public ActionResult<object> GetStats()
        {
            return Ok(new 
            { 
                totalPlayers = _playerManager.GetAllPlayers().Count,
                averageCompletionRate = _playerManager.GetAverageCompletionRate(),
                totalGamesPlayed = _playerManager.GetTotalGamesPlayed(),
                activePlayers = _playerManager.GetActivePlayers()
            });
        }
    }

    public class GameRecord
    {
        public bool Completed { get; set; }
        public int TimeSeconds { get; set; }
        public int HintsUsed { get; set; }
    }
}
