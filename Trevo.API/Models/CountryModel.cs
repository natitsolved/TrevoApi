using System.ComponentModel.DataAnnotations;

namespace Trevo.API.Models
{
    public class CountryModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Flag_Icon { get; set; }

    }
}