namespace Trevo.API.Models
{
    public class TransliterationRequestModel
    {
        public int IsTTS { get; set; }

        public int IsTranslate { get; set; }

        public int IsSpellCheck { get; set; }

        public int IsFavourite { get; set; }
        public string Details { get; set; }

        public long User_Id { get; set; }
    }
}