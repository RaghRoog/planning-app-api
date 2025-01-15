
using System.ComponentModel.DataAnnotations.Schema;

public class Goal {
	public long Id { get; set; }
	public long UserId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Priority { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime Deadline { get; set; }
	public DateTime? CompletionDate { get; set; }
	public float Progress { get; set; }
	public bool IsCompleted { get; set; }
	[NotMapped]
	public bool IsOverdue { get; set; }
}
