using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.Chat;

namespace Trevo.Services.Chat
{
    public interface IChatOfflineService
    {
        bool InsertOfflineChatDetails(ChatOfflineMessageDetails details);
        List<ChatOfflineMessageDetails> GetOfflineMessagesByUserId(long id);
        ReturnMsg DeleteOfflineMessageByUserId(long id);
    }
}
