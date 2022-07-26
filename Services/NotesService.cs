using Newtonsoft.Json;
using Notes.Api.Models;

namespace Notes.Api.Services
{
    public class NotesService : INotesService
    {
        private readonly IConfiguration _configuration;
        public NotesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddNoteAsync(Note note)
        {
            var notesList = await GetNotesList();
            notesList.Add(note);
            await SaveNotes(notesList);

        }

        public async Task DeleteNoteAsync(Guid id)
        {
            var notesList = await GetNotesList();
            if (notesList != null)
            {
               var note = notesList.First(i => i.Id == id);
                var deleted = notesList.Remove(note);
                if (deleted) {
                    await SaveNotes(notesList);
                }

            }
        }

        public async Task<IEnumerable<Note>> GetAll()
        {
            return await GetNotesList();
        }

        public async Task<Note?> GetByIdAsync(Guid id)
        {
            var notesList = await GetNotesList();
            if (notesList != null)
            {
                return notesList.FirstOrDefault(i => i.Id == id);
            }
            return null;
 
        }

        public async Task UpdateNoteAsync(Note note)
        {
            var notesList = await GetNotesList();
            if (notesList != null)
            {
                foreach (var item in notesList.Where( i => i.Id ==  note.Id))
                {
                    item.Title = note.Title;
                    item.Description = note.Description;
                }
                await SaveNotes(notesList);
            }
        }

        private async Task<List<Note>> GetNotesList()
        {
            var notesList = new List<Note>();
            var filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if(!string.IsNullOrEmpty(json))
                {
                    return await Task.Run(() => JsonConvert.DeserializeObject<List<Note>>(json));
                }
            }
            return notesList;
        }

        private string GetFilePath()
        {
            string path = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(path, _configuration["FilePath"], "notes.json");
            return filePath;
        }

        private async Task SaveNotes(List<Note> notesList)
        {
            var filePath = GetFilePath();
            await using (FileStream fs = File.Create(filePath))
            using (StreamWriter file = new StreamWriter(fs))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, notesList);
            }
        }


    }
}
