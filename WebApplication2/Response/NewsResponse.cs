namespace WebApplication2.Response;

public class NewsResponse
{
    public int Id { get; init; }
    public string? Title { get; init; }
    
    public string? ImageBase64 { get; init; }
    public string? Description { get; init; }
    public int Priority { get; init; }
}