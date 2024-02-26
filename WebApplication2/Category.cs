namespace WebApplication2;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public List<Subcategory>? Subcategories { get; set; }
}