namespace SimpleRag.Application.Services.File;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream data, string fileName, string contentType);
    Task<Stream> GetFileAsync(string fileId);
}