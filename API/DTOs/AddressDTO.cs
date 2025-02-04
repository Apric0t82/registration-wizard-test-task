using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class AddressDto
{
    // here we can add later Line1, Line2, City, State, Zip

    [Required]
    public int CountryId { get; set; }
    [Required]
    public string Country { get; set; } = string.Empty;

    [Required]
    public int ProvinceId { get; set; }
    [Required]
    public string Province { get; set; } = string.Empty;

}
