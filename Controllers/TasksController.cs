using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class TasksController : ControllerBase {
	private readonly ITaskService _taskService;

	public TasksController(ITaskService taskService) {
		_taskService = taskService;
	}

	[HttpGet("{taskId}")]
	public ActionResult GetTaskById(long taskId) {
		try {
			var task = _taskService.GetTaskById(taskId);
			return Ok(task);
		} catch (Exception ex) {
			return NotFound(new { Message = ex.Message });
		}
	}

	[HttpPost("add")]
	public ActionResult AddTask([FromBody] Task newTask) { 
		_taskService.AddTask(newTask);

		return Ok(new { Message = "Task added" });
	}

	[HttpGet("goal/{goalId}")]
	public ActionResult<IEnumerable<Task>> GetTasksByGoalId(long goalId) {
		try {
			var tasks = _taskService.GetTasksByGoalId(goalId);
			return Ok(tasks);
		}catch (Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpGet("user/{userId}")]
	public ActionResult<IEnumerable<Task>> GetAllUserTasks(long userId) {
		try {
			var tasks = _taskService.GetAllUserTasks(userId);
			return Ok(tasks);
		}catch(Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpDelete("delete/{taskId}")]
	public ActionResult DeleteTask(long taskId) {
		try {
			_taskService.DeleteTask(taskId);
			return Ok(new { Message = "Task deleted" });
		}catch (Exception ex) {
			return BadRequest (new { Message = ex.Message });
		}
	}

	[HttpPatch("update/{taskId}")]
	public ActionResult UpdateTask(long taskId, [FromBody] Dictionary<string, object> updates) {
		try {
			_taskService.UpdateTask(taskId, updates);
			return Ok(new { Message = "Task updated" });
		}catch(Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}
}
