namespace SimpleRag.Application.Interfaces;

public interface IStoreFile
{
    Task<string> SaveAsync(Stream data, string fileName, string contentType, Guid fileId);
    Task RemoveAsync(Guid fileId);
    Task<Stream> OpenReadAsync(Guid fileId);

}