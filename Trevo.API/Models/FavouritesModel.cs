using System.ComponentModel.DataAnnotations;

namespace Trevo.API.Models
{
    public class FavouritesModel
    {
        public long FavouritesId { get; set; }

        public string Message { get; set; }

        [Required]
        public long FavouriteUserId { get; set; }
        [Required]
        public int IsSender { get; set; }
        [Required]
        public long SenderRecieverId { get; set; }

        public long MomentId { get; set; }

        public string FavouritesUserName { get; set; }

        public string SenderRecieverName { get; set; }

        public string Icon_Path { get; set; }

        public string ImagePath { get; set; }

        public string AddedDate { get; set; }
        public int IsCorrected { get; set; }
        public string CorrectedText { get; set; }

        public string IncorrectedText { get; set; }
        public int LocalMessageId { get; set; }
    }
}