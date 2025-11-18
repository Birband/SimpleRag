using Google.GenAI;
using Google.GenAI.Types;

using SimpleRag.Application.DTOs;

namespace SimpleRag.Application.Services.Ask;

public class AskService : IAskService
{
    public async Task<LLMResponse> AskAsync(string? question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return new LLMResponse { Answer = "Question cannot be empty." };
        }
        
        var client = new Client();
        var response = await client.Models.GenerateContentAsync(
            model: "gemini-2.5-flash", contents: question
        );
        // #TODO: Potem popraw potencjalny błąd z odpowiedzią :))
        return new LLMResponse { Answer = response.Candidates[0].Content.Parts[0].Text };
    }
}