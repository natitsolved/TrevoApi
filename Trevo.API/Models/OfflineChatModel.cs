using System.ComponentModel.DataAnnotations;

namespace Trevo.API.Models
{
    public class OfflineChatModel
    {
        
        [Required]
        public long senderId { get; set; }

        [Required]
        public long recieverId { get; set; }

        public string message { get; set; }

        public string image { get; set; }

        public string video { get; set; }
    }
}