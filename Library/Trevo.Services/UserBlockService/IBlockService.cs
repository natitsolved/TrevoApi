using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.Block;

namespace Trevo.Services.UserBlockService
{
    public interface IBlockService
    {
        bool InsertBlockDetails(UserBlockDetails details);
        ReturnMsg DeleteBlockDetails(long blockingUserId, long blockedUserId);
        List<UserBlockWithAllInfo> GetBlockedUserListByUserId(long id);
    }
}
