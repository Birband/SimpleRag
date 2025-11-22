using SimpleRag.Application.DTOs;
using SimpleRag.Application.Interfaces;

namespace SimpleRag.Application.Services.Ask;

public class AskService : IAskService
{
    private readonly IAiClient _aiClient;

    public AskService(IAiClient aiClient)
    {
        _aiClient = aiClient;
    }

    public async Task<LLMResponse> AskAsync(string? question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return new LLMResponse { Answer = "Question cannot be empty." };
        }
        
        var answer = await _aiClient.GetAnswearAsync(question);

        return new LLMResponse { Answer = answer };
    }
}