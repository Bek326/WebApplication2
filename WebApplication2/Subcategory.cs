﻿namespace WebApplication2;

public class Subcategory
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public List<News>? NewsList { get; set; }
}