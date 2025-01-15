using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class NotesController : ControllerBase {
	private readonly INotesService _notesService;

	public NotesController(INotesService notesService) {
		_notesService = notesService;
	}

	[HttpGet("{noteId}")]
	public ActionResult GetNoteById(long noteId) {
		try {
			var note = _notesService.GetNoteById(noteId);
			return Ok(note);
		}catch (Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpPost("add")]
	public ActionResult AddNotes([FromBody] Notes newNotes) {
		_notesService.AddNotes(newNotes);

		return Ok("Note added");
	}

	[HttpGet("task/{taskId}")]
	public ActionResult<IEnumerable<Notes>> GetNotesByTaskId(long taskId) {
		try {
			var notes = _notesService.GetNotesByTaskId(taskId);
			return Ok(notes);
		}catch (Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpDelete("delete/{noteId}")]
	public ActionResult DeleteNotes(long noteId) {
		try {
			_notesService.DeleteNotes(noteId);
			return Ok(new { Message = "Note deleted" });
		}catch (Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}

	[HttpPatch("update/{noteId}")]
	public ActionResult UpdateNote(long noteId, Dictionary<string, object> updates) {
		try {
			_notesService.UpdateNotes(noteId, updates);
			return Ok(new { Message = "Note updated" });
		}catch(Exception ex) {
			return BadRequest(new { Message = ex.Message });
		}
	}
}
