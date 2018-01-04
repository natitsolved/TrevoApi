namespace Trevo.Core.Model.UserTransliteration
{
    public class TransliterationDetailsWithImage : BaseEntity
    {
        public long Id { get; set; }

        public long User_Id { get; set; }

        public string Details { get; set; }

        public int IsTTS { get; set; }

        public int IsTranslate { get; set; }

        public int IsSpellCheck { get; set; }

        public int IsFavourite { get; set; }

        public int IsMoment { get; set; }

        public string Name { get; set; }

        public long UsersId { get; set; }

        public string ImagePath { get; set; }

        public long Country_Id { get; set; }

        public string Flag_Icon { get; set; }
    }
}
