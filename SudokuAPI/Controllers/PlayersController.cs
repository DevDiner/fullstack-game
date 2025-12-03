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
        public async Task<ActionResult<List<PlayerProfile>>> GetAll()
        {
            var players = await _playerManager.GetAllPlayersAsync();
            return Ok(players);
        }

        // GET: api/players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerProfile>> GetById(int id)
        {
            var player = await _playerManager.GetPlayerAsync(id);
            if (player == null)
                return NotFound(new { message = $"Player with ID {id} not found" });
            
            return Ok(player);
        }

        // GET: api/players/username/john
        [HttpGet("username/{username}")]
        public async Task<ActionResult<PlayerProfile>> GetByUsername(string username)
        {
            var player = await _playerManager.GetPlayerByUsernameAsync(username);
            if (player == null)
                return NotFound(new { message = $"Player '{username}' not found" });
            
            return Ok(player);
        }

        // POST: api/players
        [HttpPost]
        public async Task<ActionResult<PlayerProfile>> Create([FromBody] PlayerProfile player)
        {
            var created = await _playerManager.AddPlayerAsync(player);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/players/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlayerProfile player)
        {
            var updated = await _playerManager.UpdatePlayerAsync(id, player);
            if (updated)
                return NoContent();
            
            return NotFound(new { message = $"Player with ID {id} not found" });
        }

        // POST: api/players/5/game
        [HttpPost("{id}/game")]
        public async Task<IActionResult> RecordGame(int id, [FromBody] GameRecord record)
        {
            var recorded = await _playerManager.RecordGamePlayedAsync(id, record.Completed, record.TimeSeconds, record.HintsUsed);
            if (recorded)
                return Ok(new { message = "Game recorded successfully" });
            
            return NotFound(new { message = $"Player with ID {id} not found" });
        }

        // DELETE: api/players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _playerManager.DeletePlayerAsync(id);
            if (deleted)
                return NoContent();
            
            return NotFound(new { message = $"Player with ID {id} not found" });
        }

        // GET: api/players/leaderboard?count=10
        [HttpGet("leaderboard")]
        public async Task<ActionResult<List<PlayerProfile>>> GetLeaderboard([FromQuery] int count = 10)
        {
            var players = await _playerManager.GetLeaderboardAsync(count);
            return Ok(players);
        }

        // GET: api/players/streaks?count=10
        [HttpGet("streaks")]
        public async Task<ActionResult<List<PlayerProfile>>> GetTopStreaks([FromQuery] int count = 10)
        {
            var players = await _playerManager.GetTopPlayersByStreakAsync(count);
            return Ok(players);
        }

        // GET: api/players/fastest?count=10
        [HttpGet("fastest")]
        public async Task<ActionResult<List<PlayerProfile>>> GetFastest([FromQuery] int count = 10)
        {
            var players = await _playerManager.GetFastestPlayersAsync(count);
            return Ok(players);
        }

        // GET: api/players/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStats()
        {
            var totalPlayers = (await _playerManager.GetAllPlayersAsync()).Count;
            var avgCompletionRate = await _playerManager.GetAverageCompletionRateAsync();
            var totalGames = await _playerManager.GetTotalGamesPlayedAsync();
            var activePlayers = await _playerManager.GetActivePlayersAsync();
            
            return Ok(new 
            { 
                totalPlayers,
                averageCompletionRate = avgCompletionRate,
                totalGamesPlayed = totalGames,
                activePlayers
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
