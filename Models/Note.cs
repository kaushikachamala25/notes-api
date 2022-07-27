namespace Notes.Api.Models
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

    }
}
