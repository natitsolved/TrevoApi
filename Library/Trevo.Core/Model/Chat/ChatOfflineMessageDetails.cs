namespace Trevo.Core.Model.Chat
{
    public class ChatOfflineMessageDetails : BaseEntity
    {

        public long Id { get; set; }

        public long SenderId { get; set; }

        public long RecieverId { get; set; }

        public string TextMessage { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }
    }
}
