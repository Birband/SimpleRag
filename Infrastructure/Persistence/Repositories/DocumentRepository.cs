using SimpleRag.Application.Interfaces.Persistence;
using SimpleRag.Domain.Entities;
using SimpleRag.Infrastructure.Persistence;

namespace SimpleRag.Infrastructure.Persistence.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly DocumentsDbContext _context;

    public DocumentRepository(DocumentsDbContext context)
    {
        _context = context;
    }

    public async Task AddDocumentAsync(Document document)
    {
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();
    }

    public async Task<Document?> GetDocumentByIdAsync(Guid id)
    {
        return await _context.Documents.FindAsync(id);
    }
}