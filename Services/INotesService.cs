using Notes.Api.Models;

namespace Notes.Api.Services
{
    public interface INotesService
    {
        public Task<IEnumerable<Note>> GetAll();
        public Task<Note?> GetByIdAsync(Guid id);
        public Task AddNoteAsync(Note note);
        public Task UpdateNoteAsync(Note note);
        public Task DeleteNoteAsync(Guid note);

    }
}
