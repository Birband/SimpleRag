namespace SimpleRag.Application.ExternalInterfaces;

public interface IChunkText
{
    Task<IEnumerable<string>> ChunkTextAsync(string text, int chunkSize, int overlap);
}