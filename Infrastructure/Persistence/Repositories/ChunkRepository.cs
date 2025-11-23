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

    public async Task SaveChunksAsync(Guid documentId, IEnumerable<string> chunks, IEnumerable<Vector> embeddings)
    {
        var chunkList = chunks.ToList();
        var embeddingList = embeddings.ToList();

        if (chunkList.Count != embeddingList.Count)
        {
            throw new ArgumentException("The number of chunks must match the number of embeddings.");
        }

        for (int i = 0; i < chunkList.Count; i++)
        {
            var chunkEntity = new Chunk
            {
                DocumentId = documentId,
                Text = chunkList[i],
                Embedding = embeddingList[i]
            };

            _dbContext.Chunks.Add(chunkEntity);
        }

        await _dbContext.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<(string Chunk, float Similarity)>> GetSimilarChunksAsync(Vector queryEmbedding, int topK, float maxDistance)
    {
        var items = await _dbContext.Chunks
            .Select(c => new {c.Text, Distance = c.Embedding.CosineDistance(queryEmbedding)})
            .Where(x => x.Distance <= maxDistance)
            .OrderBy(x => x.Distance)
            .Take(topK)
            .ToListAsync();

        return items.Select(i => (i.Text, (float)i.Distance));
    }
}