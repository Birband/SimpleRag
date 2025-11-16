using SimpleRag.Application.DTOs;

namespace SimpleRag.Application.Services;

public interface IAskService
{
    Task<LLMResponse> AskAsync(string question);
}