using SimpleRag.Application.Services.Rag;

namespace SimpleRag.Infrastructure.FileStorage;

public class FileSystemFileStorage : IStoreFile
{
    private readonly string _basePath;

    public FileSystemFileStorage(string basePath)
    {
        _basePath = basePath;
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public async Task<string> SaveAsync(Stream data, string fileName, string contentType)
    {
        string fileId = Guid.NewGuid().ToString();
        string filePath = Path.Combine(_basePath, fileId + "_" + fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await data.CopyToAsync(fileStream);
        }
        return fileId;
    }

    public async Task<Stream> OpenReadAsync(string fileId)
    {
        var files = Directory.GetFiles(_basePath, fileId + "_*");
        if (files.Length == 0)
        {
            throw new FileNotFoundException("File not found", fileId);
        }

        var fileStream = new FileStream(files[0], FileMode.Open, FileAccess.Read);
        return await Task.FromResult(fileStream);
    }
}