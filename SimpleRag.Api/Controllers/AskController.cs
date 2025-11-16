using Microsoft.AspNetCore.Mvc;

using SimpleRag.Api.DTOs;
using SimpleRag.Application.DTOs;
using SimpleRag.Application.Services;

namespace SimpleRag.Api.Controllers;

[ApiController]
[Route("api/ask")]
public class AskController : ControllerBase
{
    private readonly IAskService _askService;

    public AskController(IAskService askService)
    {
        _askService = askService;
    }

    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] AskRequest request)
    {
        var response = await _askService.AskAsync(request.Question!);
        return Ok(response);
    }
}