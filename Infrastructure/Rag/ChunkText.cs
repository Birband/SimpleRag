using SimpleRag.Application.Interfaces;

namespace SimpleRag.Infrastructure.Rag;

public class ChunkText : IChunkText
{
    public async Task<IEnumerable<string>> ChunkTextAsync(string text, int chunkSize, int overlap)
    {
        if (chunkSize <= 0)
            throw new ArgumentException("Chunk size must be greater than zero.", nameof(chunkSize));
        if (overlap < 0)
            throw new ArgumentException("Overlap must be non-negative.", nameof(overlap));
        if (overlap >= chunkSize)
            throw new ArgumentException("Overlap must be less than chunk size.", nameof(overlap));

        var chunks = new List<string>();
        int start = 0;
        while (start < text.Length)
        {
            int end = Math.Min(start + chunkSize, text.Length);
            string chunk = text.Substring(start, end - start);
            chunks.Add(chunk);
            start += chunkSize - overlap;
        }
        return await Task.FromResult(chunks);
    }
}