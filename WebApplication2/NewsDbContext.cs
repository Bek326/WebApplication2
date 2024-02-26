using Microsoft.EntityFrameworkCore;

namespace WebApplication2;

public class NewsDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Subcategory> Subcategories { get; set; }
    public DbSet<News> News { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }
}