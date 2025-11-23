using SimpleRag.Application.Interfaces;
using SimpleRag.Domain.Entities;


using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SimpleRag.Infrastructure.AiIntegration;

public class AiClient : IAiClient
{

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AiClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
        _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ?? "KEY";
    }
    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        string url = "v1beta/models/gemini-embedding-001:embedContent";

        var payload = new
        {
            model = "models/gemini-embedding-001",
            content = new
            {
                parts = new[]
                {
                    new { text = text }
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload);

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("x-goog-api-key", _apiKey);

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(responseContent);
        var embeddings = document.RootElement
            .GetProperty("embedding")
            .GetProperty("values")
            .Deserialize<float[]>();
            
        return embeddings ?? Array.Empty<float>();
    }
    public async Task<float[][]> GetEmbeddingsAsync(IEnumerable<string> chunks)
    {
        string url = "v1beta/models/gemini-embedding-001:batchEmbedContents";

        var chunksPerBatch = 20;

        var allEmbeddings = new List<float[]>();

        for (int i = 0; i < chunks.Count(); i += chunksPerBatch)
        {
            var batch = chunks.Skip(i).Take(chunksPerBatch);

            var requests = batch.Select(chunk => new
            {
                model = "models/gemini-embedding-001",
                content = new
                {
                    parts = new[]
                    {
                        new { text = chunk }
                    }
                }
            });

            var payload = new
            {
                requests = requests
            };

            var jsonPayload = JsonSerializer.Serialize(payload);

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("x-goog-api-key", _apiKey);

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseContent);
            var batchEmbeddings = document.RootElement
                .GetProperty("embeddings")
                .EnumerateArray()
                .Select(e => e.GetProperty("values").Deserialize<float[]>())
                .ToArray();

            allEmbeddings.AddRange(batchEmbeddings!);
        }

        return allEmbeddings.ToArray();
    }

    public async Task<string> GetAnswearAsync(string prompt)
    {
        string url = "v1beta/models/gemini-2.5-flash:generateContent";

        var payload = new
        {
            contents = new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload);

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("x-goog-api-key", _apiKey);

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(responseContent);
        var text = document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text ?? string.Empty;

    }
}