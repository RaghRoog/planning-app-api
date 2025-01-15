using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase {
	private readonly IGoalService _goalService;
	private readonly ITaskService _taskService;

	public StatisticsController(IGoalService goalService, ITaskService taskService) {
		_goalService = goalService;
		_taskService = taskService;
	}

	[HttpGet("goals/{userId}")]
	public ActionResult GetGoalStatistics(long userId, bool isCompleted, string priority, string startDate, string endDate) {
		try {
			var userGoals = _goalService.GetGoalsByUserId(userId);

			if (!string.IsNullOrEmpty(priority)) {
				userGoals = userGoals.Where(g => g.Priority != null && g.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate)) {
				DateTime start = DateTime.Parse(startDate);
				DateTime end = DateTime.Parse(endDate).AddDays(1).AddTicks(-1);
				userGoals = userGoals.Where(g => g.CompletionDate >= start && g.CompletionDate <= end);
			}

			userGoals = userGoals.Where(g => g.IsCompleted == isCompleted);

			return Ok(userGoals.Count());
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet("tasks/{userId}")]
	public ActionResult GetTaskStatistics(long userId, bool isCompleted, string startDate, string endDate) {
		try {
			var userTasks = _taskService.GetAllUserTasks(userId);

			if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate)) {
				DateTime start = DateTime.Parse(startDate);
				DateTime end = DateTime.Parse(endDate).AddDays(1).AddTicks(-1); 
				userTasks = userTasks.Where(t => t.CompletionDate >= start && t.CompletionDate <= end);
			}

			userTasks = userTasks.Where(t => t.IsCompleted == isCompleted);

			return Ok(userTasks.Count());
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}
}
