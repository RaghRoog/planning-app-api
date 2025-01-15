

public class TaskService : ITaskService {
	private readonly AppDbContext _context;

	public TaskService(AppDbContext context) {
		_context = context;
	}

	public void AddTask(Task newTask) {
		_context.Tasks.Add(newTask);
		_context.SaveChanges();
	}

	public void DeleteTask(long taskId) {
		using (var transaction = _context.Database.BeginTransaction()) {
			var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
			if (task == null) {
				throw new Exception("Task not found");
			}

			var notes = _context.Notes.Where(n => n.TaskId == task.Id).ToList();

			_context.Notes.RemoveRange(notes);
			_context.Tasks.Remove(task);
			_context.SaveChanges();

			transaction.Commit();
		}
	}


	public IEnumerable<Task> GetAllUserTasks(long userId) {
		var now = DateTime.UtcNow;
		var goals = _context.Goals.Where(g => g.UserId == userId).ToList();
		
		if (!goals.Any()) {
			throw new Exception("No goals found for user");
		}

		var tasks = _context.Tasks.Where(t => goals.Select(g => g.Id).Contains(t.GoalId)).ToList();

		if (!tasks.Any()) {
			throw new Exception("No tasks found for user");
		}

		foreach (var task in tasks) {
			task.IsOverdue = !task.IsCompleted && task.Deadline < now;
		}

		return tasks;
	}

	public Task GetTaskById(long taskId) {
		var now = DateTime.UtcNow;
		var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
		
		if (task == null) {
			throw new Exception("Task not found");
		}

		if (task.Deadline < now) {
			task.IsOverdue = true;
		}

		return task;
	}

	public IEnumerable<Task> GetTasksByGoalId(long goalId) {
		var now = DateTime.UtcNow;
		var tasks = _context.Tasks.Where(t => t.GoalId == goalId).ToList();

		if (!tasks.Any()) {
			throw new Exception("No tasks for goal");
		}

		foreach (var task in tasks) {
			task.IsOverdue = !task.IsCompleted && task.Deadline < now;
		}

		return tasks;
	}

	public void UpdateTask(long taskId, Dictionary<string, object> updates) {
		var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
		if (task == null) {
			throw new Exception("Task not found");
		}

		foreach (var update in updates) {
			switch (update.Key.ToLower()) {
				case "title":
					task.Title = update.Value.ToString();
					break;
				case "description":
					task.Description = update.Value.ToString();
					break;
				case "starttime":
					task.StartTime = DateTime.Parse(update.Value.ToString());
					break;
				case "deadline":
					task.Deadline = DateTime.Parse(update.Value.ToString());
					break;
				case "completiondate":
					if (update.Value == null || string.IsNullOrWhiteSpace(update.Value.ToString())) {
						task.CompletionDate = null;
					} else {
						task.CompletionDate = DateTime.Parse(update.Value.ToString());
					}
					break;
				case "iscompleted":
					task.IsCompleted = bool.Parse(update.Value.ToString());
					break;
				default:
					throw new Exception($"Unknow field: {update.Key}");
			}
		}

		_context.SaveChanges();
	}

}
