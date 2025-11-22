using SimpleRag.Domain.Entities;

namespace SimpleRag.Application.Interfaces;

public interface IAiClient
{
    Task<float[]> GetEmbeddingAsync(string text);
    Task<float[][]> GetEmbeddingsAsync(IEnumerable<Chunk> chunks);
    Task<string> GetAnswearAsync(string prompt);
}