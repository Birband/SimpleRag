namespace SimpleRag.Application.ExternalInterfaces;

public interface IExtractText
{
    Task<string> ExtractTextFromPDF(Stream fileStream);
    Task<string> ExtractTextFromTXT(Stream fileStream);
}
