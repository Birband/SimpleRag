using System.ComponentModel.DataAnnotations;

namespace SimpleRag.Api.DTOs;

public class AskRequest
{
    [Required]
    public string? Question { get; set; }
}