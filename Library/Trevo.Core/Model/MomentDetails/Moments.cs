namespace Trevo.Core.Model.MomentDetails
{
  public  class Moments :BaseEntity
    {

        public long MomentId { get; set; }

        public long ParentId { get; set; }
        public string Message { get; set; }
        public long PosterUserId { get; set; }

        public string PostingTime { get; set; }

        public long UserUploadedId { get; set; }
    }
}
