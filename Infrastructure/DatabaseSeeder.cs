using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly FolderContext context;

    public DatabaseSeeder(FolderContext context)
    {
        this.context = context;
    }

    public async Task InitializeAsync()
    {
        await AddDefaultCategoryAsync();
        await context.SaveChangesAsync();
    }

    private async Task AddDefaultCategoryAsync()
    {
        IEnumerable<Category> defCategories = new List<Category>()
        {
            new Category
            {
                Name = "Default",
            },
            new Category
            {
                Name = "Blue",
            }
        };

        if (!await context.Categories.AnyAsync())
            await context.Categories.AddRangeAsync(defCategories);
    }
}
