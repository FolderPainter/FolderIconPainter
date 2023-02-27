using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Contexts;
public class FolderContext : DbContext
{
    public FolderContext(DbContextOptions<FolderContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        Assembly assemblyWithConfigurations = Assembly.GetExecutingAssembly();
        builder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<CustomFolder> CustomFolders { get; set; }
}
