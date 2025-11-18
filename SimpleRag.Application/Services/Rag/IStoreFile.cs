namespace SimpleRag.Application.Services.Rag;

public interface IStoreFile
{
    Task<string> SaveAsync(Stream data, string fileName, string contentType);
    Task<Stream> OpenReadAsync(string fileId);

}