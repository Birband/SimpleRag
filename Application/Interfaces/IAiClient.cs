namespace SimpleRag.Application.Interfaces;

public interface IAiClient
{
    Task<float[]> GetEmbeddingAsync(string text);
    Task<float[][]> GetEmbeddingsAsync(IEnumerable<string> chunks);
    Task<string> GetAnswearAsync(string prompt);
}