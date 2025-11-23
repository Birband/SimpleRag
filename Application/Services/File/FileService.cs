using SimpleRag.Application.Interfaces;
using SimpleRag.Application.Interfaces.Persistence;
using SimpleRag.Domain.Entities;

using Pgvector;

namespace SimpleRag.Application.Services.File;

public class FileService : IFileService
{
    private readonly IStoreFile _storeFile;
    private readonly IChunkText _chunkText;
    private readonly IExtractText _extractText;
    private readonly IChunkRepository _chunkRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IAiClient _aiClient;

    public FileService(IStoreFile storeFile,
                        IChunkText chunkText,
                        IExtractText extractText,
                        IChunkRepository chunkRepository,
                        IDocumentRepository documentRepository,
                        IAiClient aiClient)
    {
        _storeFile = storeFile;
        _chunkText = chunkText;
        _extractText = extractText;
        _chunkRepository = chunkRepository;
        _documentRepository = documentRepository;
        _aiClient = aiClient;
    }

    public async Task<string> UploadFileAsync(Stream data, string fileName, string contentType)
    {
        var fileId = Guid.NewGuid();
        await _storeFile.SaveAsync(data, fileName, contentType, fileId);
        data.Position = 0;

        var text = await _extractText.ExtractTextFromPDF(data);
        var chunks = await _chunkText.ChunkTextAsync(text, chunkSize: 1000, overlap: 100);
        if (chunks.Count() > 100)
        {
            throw new InvalidOperationException("File is too large, please upload a smaller file.");
        }
        var embeddingsFloats = await _aiClient.GetEmbeddingsAsync(chunks);
        var embeddings = embeddingsFloats
            .Select(e => new Vector(e))
            .ToList();
        
        await _documentRepository.AddDocumentAsync(new Document
        {
            Id = fileId,
            FileName = fileName,
            ContentType = contentType,
            UploadedAt = DateTime.UtcNow
        });
        await _chunkRepository.SaveChunksAsync(fileId, chunks: chunks, embeddings: embeddings);

        return fileId.ToString();
    }

    public async Task<(Stream, Document)> GetFileAsync(Guid fileId)
    {
        var fileStream = await _storeFile.OpenReadAsync(fileId);
        var document = await _documentRepository.GetDocumentByIdAsync(fileId);
        if (document == null)
        {
            throw new FileNotFoundException("Document metadata not found", fileId.ToString());
        }

        return (fileStream, document);
    }
}