using Pgvector;

namespace SimpleRag.Application.Interfaces.Persistence;

public interface IChunkRepository
{
    Task SaveChunksAsync(Guid documentId, IEnumerable<string> chunks,  IEnumerable<Vector> embeddings);
    Task RemoveChunksByDocumentIdAsync(Guid documentId);
    Task <IEnumerable<(string Chunk, float Distance)>> GetSimilarChunksAsync(Vector queryEmbedding, int topK, float maxDistance);

}