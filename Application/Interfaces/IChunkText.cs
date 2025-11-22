namespace SimpleRag.Application.Interfaces;

public interface IChunkText
{
    Task<IEnumerable<string>> ChunkTextAsync(string text, int chunkSize, int overlap);
}