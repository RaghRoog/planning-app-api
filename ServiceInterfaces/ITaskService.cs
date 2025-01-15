
public interface ITaskService { 
	Task GetTaskById(long taskId);
	void AddTask(Task newTask);
	IEnumerable<Task> GetTasksByGoalId(long goalId);
	IEnumerable<Task> GetAllUserTasks(long userId);
	void DeleteTask(long taskId);
	void UpdateTask(long taskId, Dictionary<string, object> updates);
}
