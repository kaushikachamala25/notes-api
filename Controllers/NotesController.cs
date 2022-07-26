using Microsoft.AspNetCore.Mvc;
using Notes.Api.Models;
using Notes.Api.Services;


namespace Notes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly INotesService _notesService;
        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet]
        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _notesService.GetAll();
         }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetById(Guid id)
        {
            var note = await _notesService.GetByIdAsync(id);
            if(note == null)
            {
                return NotFound();
            }
            return note;
        }

        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody] Note note)
        {
            await _notesService.AddNoteAsync(note);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNote(Guid id, Note note)
        {
            if(id != note.Id)
            {
                return BadRequest();
            }

            var result = await _notesService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            await _notesService.UpdateNoteAsync(note);
            return Ok(note);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            var note = await _notesService.GetByIdAsync(id);
            if(note == null)
            {
                return NotFound();
            }

            await _notesService.DeleteNoteAsync(id);
            return NoContent();
        }
    }
}
