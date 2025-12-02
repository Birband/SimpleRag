using SimpleRag.Application.DTOs;
using SimpleRag.Application.Interfaces;
using SimpleRag.Application.Interfaces.Persistence;
using Pgvector;
using System.Security.Principal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace SimpleRag.Application.Services.Ask;

public class AskService : IAskService
{
    private readonly IAiClient _aiClient;
    private readonly IChunkRepository _chunkRepository;
    private readonly ApplicationSettings _appSettings;
    
    public AskService(IAiClient aiClient, IChunkRepository chunkRepository, IOptions<ApplicationSettings> appSettings)
    {
        
        _aiClient = aiClient;
        _chunkRepository = chunkRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<LLMResponse> AskAsync(string? question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return new LLMResponse { Question = question ?? string.Empty, Answer = "Question cannot be empty." };
        }
        if (question.Length > _appSettings.MaxQuestionLength)
        {
            return new LLMResponse { Question = question, Answer = $"Question is too long. Please limit to {_appSettings.MaxQuestionLength} characters." };
        }
        var embeddedQuestion = await _aiClient.GetEmbeddingAsync(question);
        var embeddedQuestionVector = new Vector(embeddedQuestion.ToArray());
        var relevantChunks = await _chunkRepository.GetSimilarChunksAsync(embeddedQuestionVector, topK: _appSettings.TopK, maxDistance: _appSettings.MaxDistance);
        Console.WriteLine($"Found {relevantChunks.Count()} relevant chunks for the question.");
        var prompt = @$"You are an assistant. Use ONLY the context below to answer.
                        Answear in the language of the question.
                        
                        ===CONTEXT START===
                        {string.Join("\n", relevantChunks.Select(c => c.Chunk))}
                        ===CONTEXT END===
                        
                        QUESTION:
                        {question}";

        var answer = await _aiClient.GetAnswearAsync(prompt);

        return new LLMResponse { Question = question, Answer = answer };
    }
}