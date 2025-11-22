using SimpleRag.Application.Interfaces;

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

    public async Task<string> SaveAsync(Stream data, string fileName, string contentType, Guid fileId)
    {
        string fileIdString = fileId.ToString();
        string filePath = Path.Combine(_basePath, fileIdString);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await data.CopyToAsync(fileStream);
        }
        return fileIdString;
    }

    public async Task<Stream> OpenReadAsync(Guid fileId)
    {   
        string fileIdString = fileId.ToString();
        var files = Directory.GetFiles(_basePath, fileIdString);
        if (files.Length == 0)
        {
            throw new FileNotFoundException("File not found", fileIdString);
        }

        var fileStream = new FileStream(files[0], FileMode.Open, FileAccess.Read);
        return await Task.FromResult(fileStream);
    }
}