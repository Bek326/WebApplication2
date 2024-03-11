using System.ComponentModel.DataAnnotations;

namespace WebApplication2;

public class Subcategory
{
    public int Id { get; set; }
    
    [StringLength(256)]
    public string? Name { get; set; }
    public int CategoryId { get; set; }
}