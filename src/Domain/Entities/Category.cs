using Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class Category : Entity
{
    [Required]
    public string Name { get; set; }

    public virtual ICollection<CustomFolder> CustomFolders { get; set; }
    
    public Category() 
    {
        CustomFolders = new HashSet<CustomFolder>();
    }

    public Category(string name)
    {
        Name = name;
        CustomFolders = new HashSet<CustomFolder>();
    }

    public Category Update(string? name)
    {
        if (name is not null && Name?.Equals(name) is not true) 
            Name = name;
        return this;
    }
}
