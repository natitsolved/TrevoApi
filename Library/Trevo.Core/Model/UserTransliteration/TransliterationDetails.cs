namespace Trevo.Core.Model.UserTransliteration
{
    public class TransliterationDetails :BaseEntity
    {
        public long Id { get; set; }

        public long User_Id { get; set; }

        public string Details { get; set; }

        public int IsTTS { get; set; }

        public int IsTranslate { get; set; }

        public int IsSpellCheck { get; set; }

        public int IsFavourite { get; set; }

        public int IsMoment { get; set; }
    }
}
