using Microsoft.AspNetCore.Mvc;
using SudokuAPI.Models;
using SudokuAPI.Services;

namespace SudokuAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PuzzlesController : ControllerBase
    {
        private readonly PuzzleManager _puzzleManager;

        public PuzzlesController(PuzzleManager puzzleManager)
        {
            _puzzleManager = puzzleManager;
        }

        // GET: api/puzzles
        [HttpGet]
        public async Task<ActionResult<List<SudokuPuzzle>>> GetAll()
        {
            var puzzles = await _puzzleManager.GetAllPuzzlesAsync();
            return Ok(puzzles);
        }

        // GET: api/puzzles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SudokuPuzzle>> GetById(int id)
        {
            var puzzle = await _puzzleManager.GetPuzzleAsync(id);
            if (puzzle == null)
                return NotFound(new { message = $"Puzzle with ID {id} not found" });
            
            return Ok(puzzle);
        }

        // GET: api/puzzles/difficulty/easy
        [HttpGet("difficulty/{difficulty}")]
        public async Task<ActionResult<List<SudokuPuzzle>>> GetByDifficulty(DifficultyLevel difficulty)
        {
            var puzzles = await _puzzleManager.GetPuzzlesByDifficultyAsync(difficulty);
            return Ok(puzzles);
        }

        // GET: api/puzzles/random?difficulty=easy
        [HttpGet("random")]
        public async Task<ActionResult<SudokuPuzzle>> GetRandom([FromQuery] DifficultyLevel? difficulty)
        {
            var puzzle = await _puzzleManager.GetRandomPuzzleAsync(difficulty);
            if (puzzle == null)
                return NotFound(new { message = "No puzzles available for the specified difficulty" });
            
            return Ok(puzzle);
        }

        // POST: api/puzzles
        [HttpPost]
        public async Task<ActionResult<SudokuPuzzle>> Create([FromBody] SudokuPuzzle puzzle)
        {
            var created = await _puzzleManager.AddPuzzleAsync(puzzle);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/puzzles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SudokuPuzzle puzzle)
        {
            var updated = await _puzzleManager.UpdatePuzzleAsync(id, puzzle);
            if (updated)
                return NoContent();
            
            return NotFound(new { message = $"Puzzle with ID {id} not found" });
        }

        // POST: api/puzzles/5/play
        [HttpPost("{id}/play")]
        public async Task<IActionResult> RecordPlay(int id, [FromBody] PlayRecord record)
        {
            var recorded = await _puzzleManager.RecordPlayAsync(id, record.Completed, record.TimeSeconds);
            if (recorded)
                return Ok(new { message = "Play recorded successfully" });
            
            return NotFound(new { message = $"Puzzle with ID {id} not found" });
        }

        // DELETE: api/puzzles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _puzzleManager.DeletePuzzleAsync(id);
            if (deleted)
                return NoContent();
            
            return NotFound(new { message = $"Puzzle with ID {id} not found" });
        }

        // GET: api/puzzles/stats/difficulty
        [HttpGet("stats/difficulty")]
        public async Task<ActionResult<Dictionary<DifficultyLevel, int>>> GetDifficultyStats()
        {
            var stats = await _puzzleManager.GetPuzzleCountByDifficultyAsync();
            return Ok(stats);
        }

        // GET: api/puzzles/top?count=10
        [HttpGet("top")]
        public async Task<ActionResult<List<SudokuPuzzle>>> GetMostPlayed([FromQuery] int count = 10)
        {
            var puzzles = await _puzzleManager.GetMostPlayedPuzzlesAsync(count);
            return Ok(puzzles);
        }

        // GET: api/puzzles/stats/completion-rate
        [HttpGet("stats/completion-rate")]
        public async Task<ActionResult<object>> GetCompletionStats()
        {
            var rate = await _puzzleManager.GetAverageCompletionRateAsync();
            return Ok(new { averageCompletionRate = rate });
        }
    }

    public class PlayRecord
    {
        public bool Completed { get; set; }
        public int TimeSeconds { get; set; }
    }
}
