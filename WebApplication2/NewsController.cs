using Microsoft.AspNetCore.Mvc;

namespace WebApplication2;

[Route("api/[controller]")]
[ApiController]

public class NewsController : ControllerBase
{
    private readonly NewsDbContext _context;

    public NewsController(NewsDbContext context)
    {
        _context = context;
    }

    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("AddSubcategory")]
    public async Task<IActionResult> AddSubcategory(Subcategory subcategory)
    {
        _context.Subcategories.Add(subcategory);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("AddNews")]
    public async Task<IActionResult> AddNews(News news)
    {
        _context.News.Add(news);
        await _context.SaveChangesAsync();
        return Ok();
    }
}