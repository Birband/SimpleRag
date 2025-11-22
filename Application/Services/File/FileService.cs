using Microsoft.AspNetCore.Razor.TagHelpers;
using SimpleRag.Application.ExternalInterfaces;

namespace SimpleRag.Application.Services.File;

public class FileService : IFileService
{
    private readonly IStoreFile _storeFile;
    private readonly IChunkText _chunkText;
    private readonly IExtractText _extractText;

    public FileService(IStoreFile storeFile, IChunkText chunkText, IExtractText extractText)
    {
        _storeFile = storeFile;
        _chunkText = chunkText;
        _extractText = extractText;
    }

    public async Task<string> UploadFileAsync(Stream data, string fileName, string contentType)
    {
        var fileId = await _storeFile.SaveAsync(data, fileName, contentType);
        data.Position = 0;

        return fileId;
    }

    public async Task<Stream> GetFileAsync(string fileId)
    {
        var fileStream = await _storeFile.OpenReadAsync(fileId);
        return fileStream;
    }
}