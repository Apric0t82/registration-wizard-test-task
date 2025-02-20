using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Core.Entities;

public class Country : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    public ICollection<Province>? Provinces { get; set; }
}
