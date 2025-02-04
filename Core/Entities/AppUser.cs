using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        public int CountryId { get; set; }
        public Country? Country { get; set; }

        [Required]
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
    }
}
