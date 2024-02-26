namespace WebApplication2;

public class News
{
    public string? Title { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public int? Priority { get; set; }
    
    public int SubcategoryId { get; set; }
    
    public Subcategory? Subcategory { get; set; }
}