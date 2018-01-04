namespace Trevo.Core.Model.MomentDetails
{
    public  class MomentDetailsWithLang :BaseEntity
    {
        public long MomentId { get; set; }

        public long ParentId { get; set; }
        public string Message { get; set; }
        public long PosterUserId { get; set; }

        public string PostingTime { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public long UserUploadedId { get; set; }

        public string Flag_Icon { get; set; }
        public long learning_Languageid { get; set; }
        public long native_languageId { get; set; }

        public string nativeAbbrv { get; set; }

        public string learningAbbrv { get; set; }

        public long FollowerUserId { get; set; }

        public long UsersLanguageId { get; set; }
    }
}
