namespace SimpleRag.Domain.Entities;

public class Document
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public ICollection<Chunk> Chunks { get; set; } = new List<Chunk>();
}