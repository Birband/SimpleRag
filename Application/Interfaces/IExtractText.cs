namespace SimpleRag.Application.Interfaces;

public interface IExtractText
{
    Task<string> ExtractTextFromPDF(Stream fileStream);
    Task<string> ExtractTextFromTXT(Stream fileStream);
}
