using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Requests;
using WebApplication2.Response;

namespace WebApplication2;

[Route("api")]
[ApiController]
public class NewsController(NewsDbContext context) : ControllerBase
{
    [HttpPost("subcategories/{subCategoryId:int}/news")]
    public async Task<IActionResult> AddNews(int subCategoryId, [FromBody] NewsRequest request)
    {
        // Проверка существования подкатегории
        var subcategory = await context.Subcategories.FindAsync(subCategoryId);
        if (subcategory == null)
        {
            return NotFound("Указанная подкатегория не найдена.");
        }

        byte[]? image = null;
        if (!string.IsNullOrEmpty(request.ImageBase64))
        {
            try
            {
                var base64Data = request.ImageBase64.Substring(request.ImageBase64.IndexOf(',') + 1);
                image = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                return BadRequest("Некорректный формат изображения.");
            }
        }

        var news = new News
        {
            Title = request.Title,
            Image = image,
            Description = request.Description,
            Priority = request.Priority,
            SubcategoryId = subCategoryId
        };

        context.News.Add(news);
        await context.SaveChangesAsync();

        return Ok(new { news.Id });
    }

    [HttpPost("categories/{categoryId:int}/subcategories")]
    public async Task<IActionResult> AddSubcategory(int categoryId, [FromBody] SubcategoryRequest request)
    {
        // Проверка существования категории
        var category = await context.Categories.FindAsync(categoryId);
        if (category == null)
        {
            return NotFound("Указанная категория не найдена.");
        }

        var subcategory = new Subcategory
        {
            Name = request.Name,
            CategoryId = categoryId
        };

        context.Subcategories.Add(subcategory);
        await context.SaveChangesAsync();

        return Ok(new { subcategory.Id });
    }

    [HttpPost("categories")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        return Ok(new { category.Id });
    }
    
    [HttpPut("subcategories/{subCategoryId:int}/news/{newsId:int}")]
    public async Task<IActionResult> UpdateNews(int subCategoryId, int newsId, [FromBody] NewsRequest request)
    {
        var news = await context.News.FindAsync(newsId);
        if (news == null || news.SubcategoryId != subCategoryId)
        {
            return NotFound("Новость не найдена или не соответствует указанной подкатегории.");
        }

        byte[]? image = null;
        if (!string.IsNullOrEmpty(request.ImageBase64))
        {
            try
            {
                var base64Data = request.ImageBase64.Substring(request.ImageBase64.IndexOf(',') + 1);
                image = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                return BadRequest("Некорректный формат изображения.");
            }
        }

        news.Title = request.Title;
        news.Image = image;
        news.Description = request.Description;
        news.Priority = request.Priority;

        await context.SaveChangesAsync();

        return Ok(new { news.Id });
    }

    [HttpPut("categories/{categoryId:int}/subcategories/{subcategoryId:int}")]
    public async Task<IActionResult> UpdateSubcategory(int categoryId, int subcategoryId, [FromBody] SubcategoryRequest request)
    {
        var subcategory = await context.Subcategories.FindAsync(subcategoryId);
        if (subcategory == null || subcategory.CategoryId != categoryId)
        {
            return NotFound("Подкатегория не найдена или не принадлежит указанной категории.");
        }

        subcategory.Name = request.Name;

        await context.SaveChangesAsync();

        return Ok(new { subcategory.Id });
    }

    [HttpPut("categories/{categoryId:int}")]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryRequest request)
    {
        var category = await context.Categories.FindAsync(categoryId);
        if (category == null)
        {
            return NotFound("Категория не найдена.");
        }

        category.Name = request.Name;

        await context.SaveChangesAsync();

        return Ok(new { category.Id });
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await context.Categories
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return Ok(categories);
    }
    
    [HttpGet("categories/{categoryId:int}/subcategories")]
    public async Task<IActionResult> GetSubcategoriesByCategory(int categoryId)
    {
        var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId);
        if (!categoryExists)
        {
            return NotFound("Категория не найдена.");
        }

        var subcategories = await context.Subcategories
            .Where(sc => sc.CategoryId == categoryId)
            .Select(sc => new SubcategoryResponse
            {
                Id = sc.Id,
                Name = sc.Name,
            })
            .ToListAsync();

        return Ok(subcategories);
    }
    
    [HttpGet("subcategories/{subCategoryId:int}/news")]
    public async Task<IActionResult> GetNewsBySubcategory(int subCategoryId)
    {
        var subcategoryExists = await context.Subcategories.AnyAsync(sc => sc.Id == subCategoryId);
        if (!subcategoryExists)
        {
            return NotFound("Подкатегория не найдена.");
        }

        var newsItems = await context.News
            .Where(n => n.SubcategoryId == subCategoryId)
            .Select(n => new NewsResponse
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Priority = n.Priority ?? 0,
                ImageBase64 = n.Image != null ? Convert.ToBase64String(n.Image) : null
            })
            .OrderBy(n => n.Priority)
            .ToListAsync();

        return Ok(newsItems);
    }
    
    [HttpGet("categories/{categoryId:int}")]
    public async Task<IActionResult> GetCategoryById(int categoryId)
    {
        var category = await context.Categories
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstOrDefaultAsync();

        if (category == null)
        {
            return NotFound("Категория не найдена.");
        }

        return Ok(category);
    }
    
    [HttpGet("subcategories/{subCategoryId:int}")]
    public async Task<IActionResult> GetSubcategoryById(int subCategoryId)
    {
        var subcategory = await context.Subcategories
            .Where(sc => sc.Id == subCategoryId)
            .Select(sc => new SubcategoryResponse
            {
                Id = sc.Id,
                Name = sc.Name
            })
            .FirstOrDefaultAsync();

        if (subcategory == null)
        {
            return NotFound("Подкатегория не найдена.");
        }

        return Ok(subcategory);
    }
    
    [HttpGet("news/{newsId:int}")]
    public async Task<IActionResult> GetNewsById(int newsId)
    {
        var news = await context.News
            .Where(n => n.Id == newsId)
            .Select(n => new NewsResponse
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Priority = n.Priority ?? 0,
                ImageBase64 = n.Image != null ? Convert.ToBase64String(n.Image) : null
            })
            .FirstOrDefaultAsync();

        if (news == null)
        {
            return NotFound("Новость не найдена.");
        }

        return Ok(news);
    }
}