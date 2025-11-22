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

    public FileService(IStoreFile storeFile, IChunkText chunkText, IExtractText extractText, IChunkRepository chunkRepository, IDocumentRepository documentRepository)
    {
        _storeFile = storeFile;
        _chunkText = chunkText;
        _extractText = extractText;
        _chunkRepository = chunkRepository;
        _documentRepository = documentRepository;
    }

    public async Task<string> UploadFileAsync(Stream data, string fileName, string contentType)
    {
        var fileId = Guid.NewGuid();
        await _storeFile.SaveAsync(data, fileName, contentType, fileId);
        data.Position = 0;

        var text = await _extractText.ExtractTextFromPDF(data);
        var chunks = await _chunkText.ChunkTextAsync(text, chunkSize: 500, overlap: 50);
        var tmp = new List<Vector>();
        // Here will be embedding generation
        foreach (var chunk in chunks)
        {
            var embedding = new Vector(new float[3072]); // Placeholder for actual embedding generation
            tmp.Add(embedding);
        }
        await _documentRepository.AddDocumentAsync(new Document
        {
            Id = fileId,
            FileName = fileName,
            ContentType = contentType,
            UploadedAt = DateTime.UtcNow
        });
        await _chunkRepository.SaveChunksAsync(fileId, chunks: chunks, embeddings: tmp);

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