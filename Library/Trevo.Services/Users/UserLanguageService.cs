using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.User;
using System.Linq;

namespace Trevo.Services.Users
{
    public class UserLanguageService : IUserLanguageService
    {
        private readonly IRepository<UsersLanguage> _userLanguageRepository;

        private const string PROC_INSERT_USER_LANGUAGE = "spl_InsertUserLanguageDetails @user_id,@learning_LanguageId,@nativeLanguageId";
        private const string PROC_UPDATE_USER_LANGUAGE = "spl_UpdateUserLanguageDetails @userLanguageId,@user_id,@learning_LanguageId,@nativeLanguageId";
        private const string PROC_GET_LANGUAGE_DETAILS_BY_USERID = "spl_GetUserLanguageDetailsByUserId @user_id";
        private const string PROC_GET_ALL_LANGUAGE = "spl_GetAllUserLanguages";
        private const string PROC_DELETE_LANGUAGE_BY_ID = "spl_DeleteLanguageById @id";

        public UserLanguageService(IRepository<UsersLanguage> userLanguageRepository)
        {
            _userLanguageRepository = userLanguageRepository;
        }

        public bool InsertUserLanguageDetails(UsersLanguage details)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@user_id",Value= details.User_Id,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@learning_LanguageId",Value = details.Learning_LanguageId,SqlDbType=SqlDbType.BigInt},
     new SqlParameter() {ParameterName = "@nativeLanguageId",Value = details.Native_LanguageId,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
         var users=   _userLanguageRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_USER_LANGUAGE, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateUserLanguageDetails(UsersLanguage details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@userLanguageId",Value= details.UsersLanguageId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@user_id",Value = details.User_Id,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@learning_LanguageId",Value = details.Learning_LanguageId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@nativeLanguageId",Value = details.Native_LanguageId,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var users = _userLanguageRepository.ExecuteStoredProcedureList<AuthUser>(PROC_UPDATE_USER_LANGUAGE, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UsersLanguage GetLanguageListByUserId(long userId)
        {
            SqlParameter param = new SqlParameter("@user_id", userId);

            var userLanguageDetails = _userLanguageRepository.ExecuteStoredProcedureList<UsersLanguage>(PROC_GET_LANGUAGE_DETAILS_BY_USERID, param).FirstOrDefault();
            return userLanguageDetails;
        }

        public IList<UsersLanguage> GetAllUsersLanguages()
        {
            var userLanguageList = _userLanguageRepository.ExecuteStoredProcedureList<UsersLanguage>(PROC_GET_ALL_LANGUAGE);
            return userLanguageList;
        }

        public ReturnMsg DeleteUserLanguage(long id)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@id", id);
                _userLanguageRepository.ExecuteStoredProcedure(PROC_DELETE_LANGUAGE_BY_ID, param);
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
