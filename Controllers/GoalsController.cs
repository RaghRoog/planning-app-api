using Microsoft.AspNetCore.Mvc;
using System.Collections;

[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase {
	private readonly IGoalService _goalService;

	public GoalsController(IGoalService goalService) {
		_goalService = goalService;
	}

	[HttpGet("{goalId}")]
	public ActionResult GetGoalById(long goalId) {
		try {
			var goal = _goalService.GetGoalById(goalId);
			return Ok(goal);
		}catch(Exception ex) {
			return NotFound(new { Message = ex.Message });
		}
	}

	[HttpPost("add")]
	public ActionResult AddGoal([FromBody] Goal newGoal) {
		try {
			_goalService.AddGoal(newGoal);
			return Ok(new { Message = "Added goal" });
		}catch(Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpGet("user/{userId}")]
	public ActionResult<IEnumerable<Goal>> GetGoalsByUserId(long userId) {
		try {
			var goals = _goalService.GetGoalsByUserId(userId);
			return Ok(goals);
		}catch(Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpDelete("delete/{goalId}")]
	public ActionResult DeleteGoal(long goalId) {
		try {
			_goalService.DeleteGoal(goalId);
			return Ok(new { Message = "Goal deleted" });
		}catch (Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpPatch("update/{goalId}")]
	public ActionResult UpdateGoal(long goalId, [FromBody] Dictionary<string, object> updates) {
		try {
			_goalService.UpdateGoal(goalId, updates);
			return Ok(new { Message = "Goal updated" });
		}catch (Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}
}
