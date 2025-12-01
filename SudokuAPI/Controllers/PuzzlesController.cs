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
        public ActionResult<List<SudokuPuzzle>> GetAll()
        {
            return Ok(_puzzleManager.GetAllPuzzles());
        }

        // GET: api/puzzles/5
        [HttpGet("{id}")]
        public ActionResult<SudokuPuzzle> GetById(int id)
        {
            var puzzle = _puzzleManager.GetPuzzle(id);
            if (puzzle == null)
                return NotFound(new { message = $"Puzzle with ID {id} not found" });
            
            return Ok(puzzle);
        }

        // GET: api/puzzles/difficulty/easy
        [HttpGet("difficulty/{difficulty}")]
        public ActionResult<List<SudokuPuzzle>> GetByDifficulty(DifficultyLevel difficulty)
        {
            return Ok(_puzzleManager.GetPuzzlesByDifficulty(difficulty));
        }

        // GET: api/puzzles/random?difficulty=easy
        [HttpGet("random")]
        public ActionResult<SudokuPuzzle> GetRandom([FromQuery] DifficultyLevel? difficulty)
        {
            var puzzle = _puzzleManager.GetRandomPuzzle(difficulty);
            if (puzzle == null)
                return NotFound(new { message = "No puzzles available for the specified difficulty" });
            
            return Ok(puzzle);
        }

        // POST: api/puzzles
        [HttpPost]
        public ActionResult<SudokuPuzzle> Create([FromBody] SudokuPuzzle puzzle)
        {
            var created = _puzzleManager.AddPuzzle(puzzle);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/puzzles/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SudokuPuzzle puzzle)
        {
            if (_puzzleManager.UpdatePuzzle(id, puzzle))
                return NoContent();
            
            return NotFound(new { message = $"Puzzle with ID {id} not found" });
        }

        // POST: api/puzzles/5/play
        [HttpPost("{id}/play")]
        public IActionResult RecordPlay(int id, [FromBody] PlayRecord record)
        {
            if (_puzzleManager.RecordPlay(id, record.Completed, record.TimeSeconds))
                return Ok(new { message = "Play recorded successfully" });
            
            return NotFound(new { message = $"Puzzle with ID {id} not found" });
        }

        // DELETE: api/puzzles/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_puzzleManager.DeletePuzzle(id))
                return NoContent();
            
            return NotFound(new { message = $"Puzzle with ID {id} not found" });
        }

        // GET: api/puzzles/stats/difficulty
        [HttpGet("stats/difficulty")]
        public ActionResult<Dictionary<DifficultyLevel, int>> GetDifficultyStats()
        {
            return Ok(_puzzleManager.GetPuzzleCountByDifficulty());
        }

        // GET: api/puzzles/top?count=10
        [HttpGet("top")]
        public ActionResult<List<SudokuPuzzle>> GetMostPlayed([FromQuery] int count = 10)
        {
            return Ok(_puzzleManager.GetMostPlayedPuzzles(count));
        }

        // GET: api/puzzles/stats/completion-rate
        [HttpGet("stats/completion-rate")]
        public ActionResult<object> GetCompletionStats()
        {
            return Ok(new { averageCompletionRate = _puzzleManager.GetAverageCompletionRate() });
        }
    }

    public class PlayRecord
    {
        public bool Completed { get; set; }
        public int TimeSeconds { get; set; }
    }
}
