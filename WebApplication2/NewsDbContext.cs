using Microsoft.EntityFrameworkCore;

namespace WebApplication2;

public class NewsDbContext : DbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
    }
    
    public DbSet<Category> Categories { get; init; } = null!;
    public DbSet<Subcategory> Subcategories { get; init; } = null!;
    public DbSet<News> News { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка NewsResponse
        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Автоинкремент для Id

            entity.HasOne<Subcategory>()
                .WithMany()
                .HasForeignKey(n => n.SubcategoryId);
        });

        // Настройка Subcategory
        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Автоинкремент для Id

            entity.HasOne<Category>()
                .WithMany()
                .HasForeignKey(sc => sc.CategoryId);
        });

        // Настройка Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Автоинкремент для Id
        });
    }
}