namespace Trevo.API.Models
{
    public class RequestModel
    {
        public string Id { get; set; }

        public string ScheduleId { get; set; }

        public int pageNo { get; set; }

        public int pageSize { get; set; }

        public string base64Image { get; set; }
        public string Content { get; set; }
        public long FollowingUserId { get; set; }
        public string NativeLang { get; set; }

        public string LearningLang { get; set; }


    }
}