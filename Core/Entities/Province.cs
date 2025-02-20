using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Province : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    public int CountryId { get; set; }

    [ForeignKey("CountryId")]
    public Country? Country { get; set; }
}
