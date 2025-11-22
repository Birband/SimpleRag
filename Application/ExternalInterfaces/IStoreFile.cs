namespace SimpleRag.Application.ExternalInterfaces;

public interface IStoreFile
{
    Task<string> SaveAsync(Stream data, string fileName, string contentType);
    Task<Stream> OpenReadAsync(string fileId);

}