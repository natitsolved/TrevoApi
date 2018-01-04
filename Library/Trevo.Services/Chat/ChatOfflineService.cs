using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.Chat;

namespace Trevo.Services.Chat
{
    public  class ChatOfflineService : IChatOfflineService
    {
        private readonly IRepository<ChatOfflineMessageDetails> _offlineChatReposiotory;

        private const string PROC_INSERT_OFFLINE_MESSAGE_DETAILS = "spl_InsertOfflineChatMessage @senderId,@recieverId,@textMessage,@imageUrl,@videoUrl";

        private const string PROC_GET_ALL_OFFLINE_MESSAGES_BY_USERID = "spl_GetOfflineChatMessageByUserId @recieverId";

        private const string PROC_DELETE_OFFLINE_MESSAGES_BY_USERID = "spl_DeleteOfflineMessageByUserId @recieverId";

        public ChatOfflineService(IRepository<ChatOfflineMessageDetails> offlineChatReposiotory)
        {
            _offlineChatReposiotory = offlineChatReposiotory;
        }

        public bool InsertOfflineChatDetails(ChatOfflineMessageDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@senderId",Value= details.SenderId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@recieverId",Value = details.RecieverId,SqlDbType=SqlDbType.BigInt},
     new SqlParameter() {ParameterName = "@textMessage",Value = details.TextMessage==null?(object)DBNull.Value:details.TextMessage,SqlDbType=SqlDbType.VarChar},
          new SqlParameter() {ParameterName = "@imageUrl",Value = details.ImageUrl==null?(object)DBNull.Value:details.ImageUrl,SqlDbType=SqlDbType.VarChar},
               new SqlParameter() {ParameterName = "@videoUrl",Value = details.VideoUrl==null?(object)DBNull.Value:details.VideoUrl,SqlDbType=SqlDbType.VarChar},
};

            object[] parameters = sp.ToArray();
            var users = _offlineChatReposiotory.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_OFFLINE_MESSAGE_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<ChatOfflineMessageDetails> GetOfflineMessagesByUserId(long id)
        {
            SqlParameter param = new SqlParameter("@recieverId", id);

            var messageList = _offlineChatReposiotory.ExecuteStoredProcedureList<ChatOfflineMessageDetails>(PROC_GET_ALL_OFFLINE_MESSAGES_BY_USERID, param).ToList();
            return messageList;
        }

        public ReturnMsg DeleteOfflineMessageByUserId(long id)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@recieverId", id);
                _offlineChatReposiotory.ExecuteStoredProcedure(PROC_DELETE_OFFLINE_MESSAGES_BY_USERID, param);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }
    }
}
