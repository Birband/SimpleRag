using Microsoft.EntityFrameworkCore;
using SimpleRag.Domain.Entities;

namespace SimpleRag.Infrastructure.Persistence;


public class DocumentsDbContext : DbContext
{
    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FileName).IsRequired();
            entity.Property(e => e.UploadedAt).IsRequired();
        });

        modelBuilder.Entity<Chunk>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Text).IsRequired();
            entity.Property(e => e.Embedding).HasColumnType("vector(768)").IsRequired();
        });
    }

    public DbSet<Document> Documents { get; set; } = null!;
    public DbSet<Chunk> Chunks { get; set; } = null!;
}

