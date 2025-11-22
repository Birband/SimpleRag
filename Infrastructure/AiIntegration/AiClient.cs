using SimpleRag.Application.Interfaces;
using SimpleRag.Domain.Entities;

using Google.GenAI;
using Google.GenAI.Types;

namespace SimpleRag.Infrastructure.AiIntegration;

public class AiClient : IAiClient
{
    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        throw new NotImplementedException();
    }
    public async Task<float[][]> GetEmbeddingsAsync(IEnumerable<Chunk> chunks)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetAnswearAsync(string prompt)
    {

        var client = new Client();
        var response = await client.Models.GenerateContentAsync(
            model: "gemini-2.5-flash", contents: prompt
        );
        return response.Candidates[0].Content.Parts[0].Text;
    }
}