using System.ComponentModel.DataAnnotations;

namespace Trevo.Core.DataRequest
{
    public class UserAuthenticationRequest
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }
    }
}
