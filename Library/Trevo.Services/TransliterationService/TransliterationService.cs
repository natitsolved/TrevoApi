using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model.UserTransliteration;

namespace Trevo.Services.TransliterationService
{
    public class TransliterationService : ITransliterationService
    {
        private readonly IRepository<TransliterationDetails> _transliterationRepository;
        private const string PROC_INSERT_TRANSLITERATION_DETAILS = "spl_InsertTransliterationDetails @user_Id,@details,@isTTS,@isTranslate,@isSpellCheck,@isFavourite,@isMoment";
        private const string PROC_GET_TRANSLITERATION_DETAILS_BY_USER_ID = "spl_GetTransliterationDetailsByUserId @user_Id";
        public TransliterationService(IRepository<TransliterationDetails> transliterationRepository)
        {
            _transliterationRepository = transliterationRepository;
        }


        public bool InsertTransliteration(TransliterationDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@user_Id",Value= details.User_Id,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@details",Value= details.Details,SqlDbType=SqlDbType.VarChar},
      new SqlParameter() {ParameterName = "@isTTS",Value= details.IsTTS,SqlDbType=SqlDbType.Int},
      new SqlParameter() {ParameterName = "@isTranslate",Value= details.IsTranslate,SqlDbType=SqlDbType.Int},
       new SqlParameter() {ParameterName = "@isSpellCheck",Value= details.IsSpellCheck,SqlDbType=SqlDbType.Int},
       new SqlParameter() {ParameterName = "@isFavourite",Value= details.IsFavourite,SqlDbType=SqlDbType.Int},
       new SqlParameter() {ParameterName = "@isMoment",Value= details.IsMoment,SqlDbType=SqlDbType.Int},
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


        public List<TransliterationDetailsWithImage> GetTransliterationDetailsByUserId(long id)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@user_Id",Value= id,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var userDetails = _transliterationRepository.ExecuteStoredProcedureList<TransliterationDetailsWithImage>(PROC_GET_TRANSLITERATION_DETAILS_BY_USER_ID, parameters).ToList();
            return userDetails;
        }
    }
}
