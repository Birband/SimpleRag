using SimpleRag.Domain.Entities;

namespace SimpleRag.Application.Interfaces.Persistence;

public interface IDocumentRepository
{
    Task AddDocumentAsync(Document document);
    Task<Document?> GetDocumentByIdAsync(Guid id);
}