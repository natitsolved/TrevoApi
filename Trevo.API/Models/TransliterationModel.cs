using System.ComponentModel.DataAnnotations;

namespace Trevo.API.Models
{
    public class TransliterationModel
    {
        public long TransliterationId { get; set; }

        [Required]
        public long User_Id { get; set; }

        public long TTSCount { get; set; }

        public long TranslateCount { get; set; }

        public long SpellCheckCount { get; set; }

        public long FavCount { get; set; }

        public int IsTTS { get; set; }

        public int IsTranslate { get; set; }

        public int IsSpellCheck { get; set; }

        public int IsFavourite { get; set; }
        public string Details { get; set; }

        public string ImagePath { get; set; }
        public string Flag_Icon { get; set; }
        public string UploadedAudioPath { get; set; }

        public string UploadedImagePath { get; set; }
        public string Message { get; set; }
    }
}