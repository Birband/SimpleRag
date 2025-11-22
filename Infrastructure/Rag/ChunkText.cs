using SimpleRag.Application.Interfaces;

namespace SimpleRag.Infrastructure.Rag;

public class ChunkText : IChunkText
{
    public async Task<IEnumerable<string>> ChunkTextAsync(string text, int chunkSize, int overlap)
    {
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