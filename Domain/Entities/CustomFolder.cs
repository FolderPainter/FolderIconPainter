using Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class CustomFolder : Entity
{
    public CustomFolder(string name, int categoryId, string colorHex)
    {
        Name = name;
        CategoryId = categoryId;
        ColorHex = colorHex;
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string ColorHex { get; set; }

    public Category Category { get; set; }

    public CustomFolder Update(string? name, int? categoryId, string? colorHex)
    {
        if (name is not null && Name?.Equals(name) is not true)
            Name = name;
        if (categoryId.HasValue && CategoryId != categoryId) CategoryId = categoryId.Value;
            CategoryId = categoryId.Value;
        if (colorHex is not null && ColorHex?.Equals(colorHex) is not true)
            ColorHex = colorHex;

        return this;
    }
}
