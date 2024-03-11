namespace WebApplication2.Requests;

public class NewsRequest
{
    public string? Title { get; init; }
    public string? ImageBase64 { get; init; } // Изменено для принятия изображения в формате base64
    public string? Description { get; init; }
    public int? Priority { get; init; }
}