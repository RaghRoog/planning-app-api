
public interface IGoalService {
	Goal GetGoalById(long id);
	void AddGoal(Goal newGoal);
	IEnumerable<Goal> GetGoalsByUserId(long userId);
	void DeleteGoal(long goalId);
	void UpdateGoal(long goalId, Dictionary<string, object> updates);
}
