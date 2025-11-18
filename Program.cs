
using SimpleRag.Application.Services.Ask;
using SimpleRag.Application.Services.File;
using SimpleRag.Infrastructure.FileStorage;
using SimpleRag.Application.Services.Rag;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IStoreFile>(
    _ => new FileSystemFileStorage(builder.Configuration.GetValue<string>("FileStorage:RootPath") ?? "XD")
);
builder.Services.AddScoped<IAskService, AskService>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();