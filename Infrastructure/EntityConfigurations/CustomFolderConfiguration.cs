using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.EnityConfiguration;
public class CustomFolderConfiguration : IEntityTypeConfiguration<CustomFolder>
{
    public void Configure(EntityTypeBuilder<CustomFolder> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired();
    }
}
