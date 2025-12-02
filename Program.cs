
using SimpleRag.Application.Services.Ask;
using SimpleRag.Application.Services.File;
using SimpleRag.Infrastructure.FileStorage;
using SimpleRag.Infrastructure.FileExtraction;
using SimpleRag.Infrastructure.Rag;
using SimpleRag.Application.Interfaces;
using SimpleRag.Infrastructure.Persistence.Repositories;
using SimpleRag.Infrastructure.Persistence;
using SimpleRag.Application.Interfaces.Persistence;
using SimpleRag.Infrastructure.AiIntegration;

using Pgvector.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleRag.Application;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

builder.Services.AddSingleton<IStoreFile, FileSystemFileStorage>();

builder.Services.AddDbContextPool<DocumentsDbContext>(options => 
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("VectorDatabase"),
        o => o.UseVector()
    )
);

builder.Services.AddScoped<IChunkRepository, ChunkRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

builder.Services.AddScoped<IAiClient, AiClient>();
builder.Services.AddScoped<IAskService, AskService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IExtractText, ExtractText>();
builder.Services.AddScoped<IChunkText, ChunkText>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();