using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.Block;

namespace Trevo.Services.UserBlockService
{
    public class BlockService : IBlockService
    {

        private readonly IRepository<UserBlockDetails> _blockRepository;

        private const string PROC_INSERT_BLOCK_DETAILS = "spl_InsertUserBlockDetails @blockingUserId,@blockedUserId,@blockingTime";
        private const string PROC_DELETE_BLOCK_DETAILS = "spl_DeleteUserBlockDetails @blockingUserId,@blockedUserId";
        private const string PROC_GET_BLOCKED_USER_LIST_BY_USER_ID = "spl_GetUserListByBlockingUserId @blockingUserId";
        public BlockService(IRepository<UserBlockDetails> blockRepository)
        {
            _blockRepository = blockRepository;
        }
        public bool InsertBlockDetails(UserBlockDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@blockingUserId",Value= details.BlockingUserId,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@blockedUserId",Value= details.BlockedUserId,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@blockingTime",Value= details.BlockingTime,SqlDbType=SqlDbType.VarChar},
};

            object[] parameters = sp.ToArray();
            var users = _blockRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_BLOCK_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ReturnMsg DeleteBlockDetails(long blockingUserId, long blockedUserId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@blockingUserId",Value= blockingUserId,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@blockedUserId",Value= blockedUserId,SqlDbType=SqlDbType.BigInt},
};
                object[] parameters = sp.ToArray();
                _blockRepository.ExecuteStoredProcedure(PROC_DELETE_BLOCK_DETAILS, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }


        public List<UserBlockWithAllInfo> GetBlockedUserListByUserId(long id)
        {
            SqlParameter param = new SqlParameter("@blockingUserId", id);

            var blockedList = _blockRepository.ExecuteStoredProcedureList<UserBlockWithAllInfo>(PROC_GET_BLOCKED_USER_LIST_BY_USER_ID, param).ToList();
            return blockedList;
        }
    }
}
