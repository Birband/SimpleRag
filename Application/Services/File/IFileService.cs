using SimpleRag.Domain.Entities;

namespace SimpleRag.Application.Services.File;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream data, string fileName, string contentType);
    Task<(Stream, Document)> GetFileAsync(Guid fileId);
}