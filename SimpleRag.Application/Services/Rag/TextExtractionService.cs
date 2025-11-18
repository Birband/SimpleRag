namespace SimpleRag.Application.Services.Rag;

public class TextExtractionService
{
    public string ExtractTextFromPdf(Stream pdfStream)
    {
        return "Extracted text from PDF.";
    }

    private List<string> SplitTextIntoChunks(string text, int chunkSize = 500)
    {
        var chunks = new List<string>();
        for (int i = 0; i < text.Length; i += chunkSize)
        {
            chunks.Add(text.Substring(i, Math.Min(chunkSize, text.Length - i)));
        }
        return chunks;
    }
}

