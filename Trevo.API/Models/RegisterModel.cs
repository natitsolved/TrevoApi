using System.ComponentModel.DataAnnotations;

namespace Trevo.API.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
       
        public string Password { get; set; }
        [Required]
        public string Dob { get; set; }
        [Required]
        [MaxLength(15)]
        public string Gender { get; set; }
        [Required]
        public string DeviceId { get; set; }
        public string SelfIntroduction { get; set; }
        [Required]
        public long CountryId { get; set; }
        public string Address { get; set; }
        public string Interests { get; set; }
        public string TravelDestination { get; set; }
        public long LanguageLevelId { get; set; }
        
        public long User_Id { get; set; }
        public long NativeLanguageId { get; set; }

        public long LearningLanguageId { get; set; }

        public string ExternalAuthType { get; set; }

        public string ExternalAuthUserId { get; set; }
    }
}