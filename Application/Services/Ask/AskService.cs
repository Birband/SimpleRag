using SimpleRag.Application.DTOs;
using SimpleRag.Application.Interfaces;
using SimpleRag.Application.Interfaces.Persistence;
using Pgvector;

namespace SimpleRag.Application.Services.Ask;

public class AskService : IAskService
{
    private readonly IAiClient _aiClient;
    private readonly IChunkRepository _chunkRepository;
    public AskService(IAiClient aiClient, IChunkRepository chunkRepository)
    {
        _aiClient = aiClient;
        _chunkRepository = chunkRepository;
    }

    public async Task<LLMResponse> AskAsync(string? question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return new LLMResponse { Answer = "Question cannot be empty." };
        }
        var embeddedQuestion = await _aiClient.GetEmbeddingAsync(question);
        var embeddedQuestionVector = new Vector(embeddedQuestion.ToArray());
        var relevantChunks = await _chunkRepository.GetSimilarChunksAsync(embeddedQuestionVector, topK: 10, maxDistance: 0.5f);
        var prompt = @$"You are an assistant. Use ONLY the context below to answer.
                        Answear in the language of the question.
                        
                        ===CONTEXT START===
                        {string.Join("\n", relevantChunks.Select(c => c.Chunk))}
                        ===CONTEXT END===
                        
                        QUESTION:
                        {question}";

        var answer = await _aiClient.GetAnswearAsync(prompt);

        return new LLMResponse { Answer = answer };
    }
}