using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace WebApplication2;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        
        services.AddDbContext<NewsDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        
        // Добавляем Swagger генератор
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication2 API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthorization();
        
        // Включаем middleware для обслуживания сгенерированного Swagger как JSON endpoint.
        app.UseSwagger();

        // Включаем middleware для обслуживания Swagger-ui (HTML, JS, CSS и т.д.),
        // указывая Swagger JSON endpoint.
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication2 API V1"));


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NewsDbContext>();
        dbContext.Database.Migrate();
    }
}