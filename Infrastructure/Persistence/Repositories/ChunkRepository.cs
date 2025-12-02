using SimpleRag.Application.Interfaces.Persistence;
using Pgvector;
using SimpleRag.Domain.Entities;
using Pgvector.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UglyToad.PdfPig.DocumentLayoutAnalysis;

namespace SimpleRag.Infrastructure.Persistence.Repositories;

public class ChunkRepository : IChunkRepository
{
    private readonly DocumentsDbContext _dbContext;

    public ChunkRepository(DocumentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static Vector NormalizeVector(Vector vector)
    {
        var values = vector.ToArray();
        float magnitude = (float)Math.Sqrt(values.Select(v => v * v).Sum());
        if (magnitude == 0) return vector;
        var normalizedValues = values.Select(v => v / magnitude).ToArray();
        return new Vector(normalizedValues);
    }

    public async Task SaveChunksAsync(Guid documentId, IEnumerable<string> chunks, IEnumerable<Vector> embeddings)
    {
        var chunkList = chunks.ToList();
        var embeddingList = embeddings.Select(NormalizeVector).ToList();

        if (chunkList.Count != embeddingList.Count)
        {
            throw new ArgumentException("The number of chunks must match the number of embeddings.");
        }

        var chunkEntities = new List<Chunk>(chunkList.Select((chunk, index) => new Chunk
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            Text = chunk,
            Embedding = embeddingList[index]
        }));
        await _dbContext.Chunks.AddRangeAsync(chunkEntities);

        await _dbContext.SaveChangesAsync();
        
    }

    public async Task RemoveChunksByDocumentIdAsync(Guid documentId)
    {
        var chunksToRemove = _dbContext.Chunks.Where(c => c.DocumentId == documentId);
        _dbContext.Chunks.RemoveRange(chunksToRemove);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<(string Chunk, float Distance)>> GetSimilarChunksAsync(Vector queryEmbedding, int topK, float maxDistance)
    {
        queryEmbedding = NormalizeVector(queryEmbedding);
        var items = await _dbContext.Chunks
            .Select(c => new {c.Text, Distance = c.Embedding.CosineDistance(queryEmbedding)})
            .Where(x => x.Distance <= maxDistance)
            .OrderBy(x => x.Distance)
            .Take(topK)
            .ToListAsync();

        return items.Select(i => (i.Text, (float)i.Distance));
    }
}