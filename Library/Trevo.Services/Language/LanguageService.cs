using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.Language;
using System.Linq;

namespace Trevo.Services.Language
{
    public class LanguageService : ILanguageService
    {
        private readonly IRepository<LanguageDetails> _languageRepository;

        private const string PROC_INSERT_LANGUAGE_DETAILS = "spl_InsertLanguageDetails @name,@imagePath,@abbrv";
        private const string PROC_UPDATE_LANGUAGE_DETAILS = "spl_UpdateLanguageDetails @name,@language_id,@imagePath,@abbrv";
        private const string PROC_GET_USER_LANGUAGE_BY_ID = "spl_GetUserLanguageDetailsById @language_id";
        private const string PROC_DELETE_LANGUAGE_BY_ID = "spl_DeleteLanguageDetailsById @lag_Id";
        private const string PROC_GET_ALL_LANGUAGES = "spl_GetAllLanguages";
        public LanguageService(IRepository<LanguageDetails> languageRepository)
        {
            _languageRepository = languageRepository;
        }
        public bool InsertLanguageDetails(LanguageDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.VarChar},
      new SqlParameter() {ParameterName = "@imagePath",Value= details.ImagePath,SqlDbType=SqlDbType.VarChar},
       new SqlParameter() {ParameterName = "@abbrv",Value= details.Abbreviation,SqlDbType=SqlDbType.VarChar},
};

            object[] parameters = sp.ToArray();
            var users = _languageRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_LANGUAGE_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateLanguageDetails(LanguageDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@language_id",Value = details.Language_Id,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@imagePath",Value= details.ImagePath,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@abbrv",Value= details.Abbreviation,SqlDbType=SqlDbType.VarChar},

};

            object[] parameters = sp.ToArray();
            var users = _languageRepository.ExecuteStoredProcedureList<AuthUser>(PROC_UPDATE_LANGUAGE_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public LanguageDetails GetLanguageDetailsById(long languageId)
        {
            SqlParameter param = new SqlParameter("@language_id", languageId);

            var languageDetails = _languageRepository.ExecuteStoredProcedureList<LanguageDetails>(PROC_GET_USER_LANGUAGE_BY_ID, param).FirstOrDefault();
            return languageDetails;
        }

        public ReturnMsg DeleteLanguage(long languageId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@lag_Id", languageId);
                _languageRepository.ExecuteStoredProcedure(PROC_DELETE_LANGUAGE_BY_ID, param);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }

        public IList<LanguageDetails> GetAllLangugaes()
        {
            var languageList = _languageRepository.ExecuteStoredProcedureList<LanguageDetails>(PROC_GET_ALL_LANGUAGES);
            return languageList;
        }
    }
}
