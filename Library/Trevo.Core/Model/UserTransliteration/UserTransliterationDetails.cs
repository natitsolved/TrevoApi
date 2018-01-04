namespace Trevo.Core.Model.UserTransliteration
{
    public class UserTransliterationDetails :BaseEntity
    {

        public long TransliterationId { get; set; }

        public long User_Id { get; set; }

        public long TTSCount { get; set; }

        public long TranslateCount { get; set; }

        public long SpellCheckCount { get; set; }

        public long FavouritesCount { get; set; }
    }
}
