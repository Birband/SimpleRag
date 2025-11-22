using SimpleRag.Application.Interfaces;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using System.Text;

namespace SimpleRag.Infrastructure.FileExtraction;

public class ExtractText : IExtractText
{

    public async Task<string> ExtractTextFromPDF(Stream fileStream)
    {
        using var pdf = PdfDocument.Open(fileStream);
        var text = new StringBuilder();
        foreach (var page in pdf.GetPages())
        {
            string pageText = ContentOrderTextExtractor.GetText(page);
            text.AppendLine(pageText);
            text.AppendLine();
        }

        string cleanedText = text.ToString().Replace("\n", " ").Replace("\r", " ").Trim();
        return await Task.FromResult(cleanedText);
    }

    public async Task<string> ExtractTextFromTXT(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        string content = await reader.ReadToEndAsync();
        return content;
    }
}