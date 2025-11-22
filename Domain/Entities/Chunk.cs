using Pgvector;

namespace SimpleRag.Domain.Entities;

public class Chunk
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string Text { get; set; } = string.Empty;
    public Vector Embedding { get; set; } = null!;
    public Document Document { get; set; } = null!;

}