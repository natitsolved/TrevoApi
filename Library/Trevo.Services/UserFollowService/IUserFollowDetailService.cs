using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.UserFollower;

namespace Trevo.Services.UserFollowService
{
    public  interface IUserFollowDetailService
    {
        bool InsertUserFollowingDetails(UserFollowerDetails details);
        ReturnMsg RemoveUserFollowDetailsByUserId(long followerId, long followingId);
        List<UserFollowDetailsWithUserName> GetFollowerListByUserId(long followingUserId);
        List<UserFollowDetailsWithUserName> GetFollowingListByUserId(long followerId);
        List<UserFollowerDetails> GetAllUserFollowList();
    }
}
