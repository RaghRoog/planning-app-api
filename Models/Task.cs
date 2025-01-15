
using System.ComponentModel.DataAnnotations.Schema;

public class Task {
	public long Id { get; set; }
	public long GoalId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime Deadline { get; set; }
	public DateTime? CompletionDate { get; set; }
	public bool IsCompleted { get; set; }
	[NotMapped]
	public bool IsOverdue { get; set; }
}
