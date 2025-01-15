

using System.Threading.Tasks;

public class NotesService : INotesService {
	private readonly AppDbContext _context;

	public NotesService(AppDbContext context) {
		_context = context;
	}

	public void AddNotes(Notes newNotes) {
		_context.Notes.Add(newNotes);
		_context.SaveChanges();
	}

	public void DeleteNotes(long noteId) {
		var note = _context.Notes.FirstOrDefault(n  => n.Id == noteId);

		if(note == null) {
			throw new Exception("Note not found");
		}

		_context.Notes.Remove(note);
		_context.SaveChanges();
	}


	public Notes GetNoteById(long id) {
		var note = _context.Notes.FirstOrDefault(n => n.Id ==  id);

		if(note == null) {
			throw new Exception("Note not found");
		}

		return note;
	}

	public IEnumerable<Notes> GetNotesByTaskId(long taskId) {
		var notes = _context.Notes.Where(n => n.TaskId == taskId).ToList();

		if (!notes.Any()) {
			throw new Exception("No notes for task found");
		}

		return notes;
	}

	public void UpdateNotes(long noteId, Dictionary<string, object> updates) {
		var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
		if (note == null) {
			throw new Exception("Note not found");
		}

		foreach (var update in updates) {
			switch (update.Key.ToLower()) {
				case "content":
					note.Content = update.Value.ToString();
					break;
				default:
					throw new Exception($"Unknow field: {update.Key}");
			}
		}

		_context.SaveChanges();
	}
}