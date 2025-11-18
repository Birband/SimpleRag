
using SimpleRag.Application.Services.Rag;

namespace SimpleRag.Application.Services.File;

public class FileService : IFileService
{
    private readonly IStoreFile _storeFile;

    public FileService(IStoreFile storeFile)
    {
        _storeFile = storeFile;
    }

    public async Task<string> UploadFileAsync(Stream data, string fileName, string contentType)
    {
        var fileId = await _storeFile.SaveAsync(data, fileName, contentType);
        return fileId;
    }

    public async Task<Stream> GetFileAsync(string fileId)
    {
        var fileStream = await _storeFile.OpenReadAsync(fileId);
        return fileStream;
    }
}