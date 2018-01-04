using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.UserFollower;

namespace Trevo.Services.UserFollowService
{
    public class UserFollowDetailService : IUserFollowDetailService
    {
        private readonly IRepository<UserFollowerDetails> _userFollowRepository;
        private const string PROC_GET_ALL_USER_FOLLOW_LIST = "spl_GetAllUserFollowDetails ";
        private const string PROC_INSERT_USER_FOLLOWING_DETAILS = "spl_InsertUserFollowers @followerUserId,@followingUserId";
        private const string PROC_GET_FOLLOWER_LIST_BY_USERID = "spl_GetFollowersListByUserId @followingUserId";
        private const string PROC_GET_FOLLOWING_LIST_BY_USERID = "spl_GetFollowingListByUserId @followerId";
        private const string PROC_DELETE_USER_FOLLOW_DETAILS_BY_ID = "spl_RemoveUserFollower @followerUserId,@followingUserId";
        public UserFollowDetailService(IRepository<UserFollowerDetails> userFollowRepository)
        {
            _userFollowRepository = userFollowRepository;
        }

        public bool InsertUserFollowingDetails(UserFollowerDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@followerUserId",Value= details.FollowerUserId,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@followingUserId",Value= details.FollowingUserId,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var users = _userFollowRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_USER_FOLLOWING_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ReturnMsg RemoveUserFollowDetailsByUserId(long followerId, long followingId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@followerUserId",Value= followerId,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@followingUserId",Value= followingId,SqlDbType=SqlDbType.BigInt},
};
                object[] parameters = sp.ToArray();
                _userFollowRepository.ExecuteStoredProcedure(PROC_DELETE_USER_FOLLOW_DETAILS_BY_ID, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }

        public List<UserFollowDetailsWithUserName> GetFollowerListByUserId(long followingUserId)
        {
            SqlParameter param = new SqlParameter("@followingUserId", followingUserId);

            var userFollowList = _userFollowRepository.ExecuteStoredProcedureList<UserFollowDetailsWithUserName>(PROC_GET_FOLLOWER_LIST_BY_USERID, param).ToList();
            return userFollowList;
        }
        public List<UserFollowDetailsWithUserName> GetFollowingListByUserId(long followerId)
        {
            SqlParameter param = new SqlParameter("@followerId", followerId);

            var userFollowList = _userFollowRepository.ExecuteStoredProcedureList<UserFollowDetailsWithUserName>(PROC_GET_FOLLOWING_LIST_BY_USERID, param).ToList();
            return userFollowList;
        }

        public List<UserFollowerDetails> GetAllUserFollowList()
        {
            var userFollowList = _userFollowRepository.ExecuteStoredProcedureList<UserFollowerDetails>(PROC_GET_ALL_USER_FOLLOW_LIST).ToList();
            return userFollowList;
        }
    }
}
