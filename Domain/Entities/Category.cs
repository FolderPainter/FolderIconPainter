using Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class Category : Entity
{
    public Category() 
    {
        CustomFolders = new HashSet<CustomFolder>();
    }

    [Required]
    public string Name { get; set; }

    public virtual ICollection<CustomFolder> CustomFolders { get; set; }
}
