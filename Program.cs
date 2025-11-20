
using SimpleRag.Application.Services.Ask;
using SimpleRag.Application.Services.File;
using SimpleRag.Infrastructure.FileStorage;
using SimpleRag.Infrastructure.FileExtraction;
using SimpleRag.Infrastructure.Rag;
using SimpleRag.Application.ExternalInterfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IStoreFile>(
    _ => new FileSystemFileStorage(builder.Configuration.GetValue<string>("FileStorage:RootPath") ?? "XD")
);
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