using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleRag.Application.Services.File;

namespace SimpleRag.Api.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        if (file.ContentType != "application/pdf" && file.ContentType != "text/plain")
        {
            return BadRequest("Only PDF and TXT files are allowed.");
        }

        using var stream = file.OpenReadStream();
        var fileId = await _fileService.UploadFileAsync(stream, file.FileName, file.ContentType);
        return Ok(new { FileId = fileId });
    }

    [HttpGet("{fileId}")]
    public async Task<IActionResult> GetFile(Guid fileId)
    {
        var (fileStream, document) = await _fileService.GetFileAsync(fileId);
        if (fileStream == null)
        {
            return NotFound();
        }

        return File(fileStream, document.ContentType, document.FileName);
    }
}