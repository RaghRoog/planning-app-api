
public interface INotesService {
	Notes GetNoteById(long id);
	void AddNotes(Notes newNotes);
	IEnumerable<Notes> GetNotesByTaskId(long taskId);	
	void DeleteNotes(long noteId);
	void UpdateNotes(long noteId, Dictionary<string, object> updates);
}
