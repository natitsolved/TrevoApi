using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.UserTransliteration;

namespace Trevo.Services.TransliterationService
{
    public class UserTransliterationService : IUserTransliterationService
    {
        private readonly IRepository<UserTransliterationDetails> _transliterationRepository;

        private const string PROC_INSERT_TRANSLITERATION_DETAILS = "spl_InsertUserTransliteration @user_Id,@ttsCount,@translateCount,@spellCheckCount,@favCount";

        private const string PROC_UPDATE_TRANSLITERATION_DETAILS = "spl_UpdateUserTransliteration @user_Id,@ttsCount,@translateCount,@spellCheckCount,@transliterationId,@favCount";
        private const string PROC_GET_TRANSLITERATION_DETAILS_BY_USER_ID = "spl_GetUserTransliterationByUserId @user_Id";
        public UserTransliterationService(IRepository<UserTransliterationDetails> transliterationRepository)
        {
            _transliterationRepository = transliterationRepository;
        }

        public bool InsertTransliterationDetails(UserTransliterationDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@user_Id",Value= details.User_Id,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@ttsCount",Value= details.TTSCount,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@translateCount",Value= details.TranslateCount,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@spellCheckCount",Value= details.SpellCheckCount,SqlDbType=SqlDbType.BigInt},
       new SqlParameter() {ParameterName = "@favCount",Value= details.FavouritesCount,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var users = _transliterationRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_TRANSLITERATION_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public UserTransliterationDetails GetTransliterationDetailsByUserId(long id)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@user_Id",Value= id,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var userDetails = _transliterationRepository.ExecuteStoredProcedureList<UserTransliterationDetails>(PROC_GET_TRANSLITERATION_DETAILS_BY_USER_ID, parameters).FirstOrDefault();
            return userDetails;
        }


        public ReturnMsg UpdateTransliterationByUserId(UserTransliterationDetails details )
        {
            ReturnMsg msg = new ReturnMsg();

            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@user_Id",Value= details.User_Id,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@ttsCount",Value= details.TTSCount,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@translateCount",Value= details.TranslateCount,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@spellCheckCount",Value= details.SpellCheckCount,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@transliterationId",Value= details.TransliterationId,SqlDbType=SqlDbType.BigInt},
             new SqlParameter() {ParameterName = "@favCount",Value= details.FavouritesCount,SqlDbType=SqlDbType.BigInt},
};

                object[] parameters = sp.ToArray();
                _transliterationRepository.ExecuteStoredProcedure(PROC_UPDATE_TRANSLITERATION_DETAILS, parameters);
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
