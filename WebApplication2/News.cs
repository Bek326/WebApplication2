using System.ComponentModel.DataAnnotations;

namespace WebApplication2;

public class News
{
    public int Id { get; set; }

    [StringLength(1024)]
    public string? Title { get; set; }

    public byte[]? Image { get; set; }
    
    [StringLength(1024)]
    public string? Description { get; set; }
    
    public int? Priority { get; set; }
    
    public int SubcategoryId { get; set; }
}