using Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class CustomFolder : Entity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public int CategoryId { get; set; } 
    
    public Category Category { get; set; }
}
