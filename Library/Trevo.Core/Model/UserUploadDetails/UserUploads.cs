namespace Trevo.Core.Model.UserUploadDetails
{
   public class UserUploads :BaseEntity
    {
        public long UserUploadsId { get; set; }

        public long UserId { get; set; }

        public string UploadedImagePath { get; set; }

        public string UploadedAudioPath { get; set; }
    }
}
