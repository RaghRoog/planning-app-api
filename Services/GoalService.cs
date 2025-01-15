

public class GoalService : IGoalService {
	private readonly AppDbContext _context;

	public GoalService(AppDbContext context) {
		_context = context;
	}

	public void AddGoal(Goal newGoal) {
		if (_context.Goals.Any(g => g.UserId == newGoal.UserId && g.Title == newGoal.Title)) {
			throw new Exception("Goal already exists");
		}

		_context.Goals.Add(newGoal);
		_context.SaveChanges();
	}

	public void DeleteGoal(long goalId) {
		using(var transaction =  _context.Database.BeginTransaction()) {
			var goal = _context.Goals.FirstOrDefault(g => g.Id == goalId);
			if (goal == null) {
				throw new Exception("Goal not found");
			}

			var tasks = _context.Tasks.Where(t => t.GoalId == goalId).ToList();

			var notes = _context.Notes.Where(n => tasks.Select(t => t.Id).Contains(n.TaskId)).ToList();

			_context.Notes.RemoveRange(notes);
			_context.Tasks.RemoveRange(tasks);
			_context.SaveChanges();
			_context.Goals.Remove(goal);
			_context.SaveChanges();

			transaction.Commit();
		}
	}


	public Goal GetGoalById(long id) {
		var goal = _context.Goals.FirstOrDefault(g => g.Id == id);
		var now = DateTime.UtcNow;

		if(goal == null) {
			throw new Exception("Goal not found");
		}

		if(goal.Deadline < now) {
			goal.IsOverdue = true;
		}

		return goal;
	}

	public IEnumerable<Goal> GetGoalsByUserId(long userId) {
		var goals = _context.Goals.Where(g => g.UserId == userId).ToList();
		var now = DateTime.UtcNow;

		if (!goals.Any()) {
			throw new Exception("No goals found");
		}

		foreach (var goal in goals) {
			goal.IsOverdue = !goal.IsCompleted && goal.Deadline < now;
		}

		return goals;
	}

	public void UpdateGoal(long goalId, Dictionary<string, object> updates) {
		var goal = _context.Goals.FirstOrDefault(g => g.Id == goalId);
		if (goal == null) {
			throw new Exception("Goal not found");
		}

		foreach (var update in updates) { 
			switch (update.Key.ToLower()) {
				case "title":
					goal.Title = update.Value.ToString();
					break;
				case "description":
					goal.Description = update.Value.ToString();
					break;
				case "priority":
					goal.Priority = update.Value.ToString();
					break;
				case "starttime":
					goal.StartTime = DateTime.Parse(update.Value.ToString());
					break;
				case "deadline":
					goal.Deadline = DateTime.Parse(update.Value.ToString());
					break;
				case "progress":
					goal.Progress = float.Parse(update.Value.ToString());	
					break;
				case "completiondate":
					if(update.Value == null || string.IsNullOrWhiteSpace(update.Value.ToString())) {
						goal.CompletionDate = null;
					} else {
						goal.CompletionDate = DateTime.Parse(update.Value.ToString());
					}
					break;
				case "iscompleted":
					goal.IsCompleted = bool.Parse(update.Value.ToString());
					break;
				default:
					throw new Exception($"Unknow field: {update.Key}");
			}
		}

		_context.SaveChanges();
	}
}