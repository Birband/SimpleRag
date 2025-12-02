namespace SimpleRag.Application;

public class ApplicationSettings
{
    public int MaxQuestionLength { get; set; } = 1000;
    public int TopK { get; set; } = 5;
    public float MaxDistance { get; set; } = 1f;
    public string BaseStoragePath { get; set; } = "uploads_default";
}