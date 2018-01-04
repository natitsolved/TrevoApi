namespace Trevo.API.Models
{
    public class MomentDetailsList
    {
        public long MomentId { get; set; }

        public long ParentId { get; set; }
        public string Message { get; set; }
        public long PosterUserId { get; set; }

        public string PostingTime { get; set; }

        public string Name { get; set; }

        public string NativeLangAbbrev { get; set; }

        public string LearningLangAbbrev { get; set; }

        public string ImagePath { get; set; }

        public string UploadedImage { get; set; }
        public string UploadedAudio { get; set; }

        public string CountryImagePath { get; set; }

        public int RepliesCount { get; set; }

        public int FavouritesCount{get;set;}
    }
}