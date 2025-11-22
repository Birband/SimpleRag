using SimpleRag.Application.DTOs;

namespace SimpleRag.Application.Services.Ask;

public interface IAskService
{
    Task<LLMResponse> AskAsync(string question);
}