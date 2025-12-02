using SimpleRag.Application.Interfaces;
using SimpleRag.Application;
using Microsoft.Extensions.Options;


namespace SimpleRag.Infrastructure.FileStorage;

public class FileSystemFileStorage : IStoreFile
{
    private readonly ApplicationSettings _appSettings;

    public FileSystemFileStorage(IOptions<ApplicationSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        if (!Directory.Exists(_appSettings.BaseStoragePath))
        {
            Directory.CreateDirectory(_appSettings.BaseStoragePath);
        }
    }

    public async Task<string> SaveAsync(Stream data, string fileName, string contentType, Guid fileId)
    {
        string fileIdString = fileId.ToString();
        string filePath = Path.Combine(_appSettings.BaseStoragePath, fileIdString);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await data.CopyToAsync(fileStream);
        }
        return fileIdString;
    }

    public async Task RemoveAsync(Guid fileId)
    {
        string fileIdString = fileId.ToString();
        string filePath = Path.Combine(_appSettings.BaseStoragePath, fileIdString);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        await Task.CompletedTask;
    }

    public async Task<Stream> OpenReadAsync(Guid fileId)
    {   
        string fileIdString = fileId.ToString();
        var files = Directory.GetFiles(_appSettings.BaseStoragePath, fileIdString);
        if (files.Length == 0)
        {
            throw new FileNotFoundException("File not found", fileIdString);
        }

        var fileStream = new FileStream(files[0], FileMode.Open, FileAccess.Read);
        return await Task.FromResult(fileStream);
    }
}