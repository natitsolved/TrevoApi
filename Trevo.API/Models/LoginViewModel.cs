using System.ComponentModel.DataAnnotations;

namespace Trevo.API.Models
{
    public class LoginViewModel
    {


        //public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        [Required]
        public string Email { get; set; }

        [Required]
        public string DeviceId { get; set; }

       
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

    }
}