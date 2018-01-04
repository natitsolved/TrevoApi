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
    public class LanguageLevelService :ILanguageLevelService
    {
        private readonly IRepository<LanguageLevel> _languageLevelRepository;

        private const string PROC_INSERT_LANGUAGE_LEVEL_DETAILS = "spl_InsertLanguageLevel @name";
        private const string PROC_UPDATE_LANGUAGE_LEVEL_DETAILS = "spl_UpdateLanguageLevel @name,@lagLevel_Id";
        private const string PROC_GET_LANGUAGE_LEVEL_DETAILS_BY_ID = "spl_GetLanguageDetailsById @lagLevel_Id";
        private const string PROC_DELETE_LANGUAGE_LEVEL_DETAILS_BY_ID = "spl_DeleteLanguageLevelDetailsById @lagLevel_Id";
        private const string PROC_GET_ALL_LANGUAGE_LEVELS = "spl_GetAllLanguageLevels";
        public LanguageLevelService(IRepository<LanguageLevel> languageLevelRepository)
        {
            _languageLevelRepository = languageLevelRepository;
        }



        public bool InsertLanguageLevelDetails(LanguageLevel details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.NChar},
};

            object[] parameters = sp.ToArray();
            var users = _languageLevelRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_LANGUAGE_LEVEL_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateLanguageLevelDetails(LanguageLevel details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.NChar},
    new SqlParameter() {ParameterName = "@lagLevel_Id",Value= details.LagLevel_Id,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var users = _languageLevelRepository.ExecuteStoredProcedureList<AuthUser>(PROC_UPDATE_LANGUAGE_LEVEL_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public LanguageLevel GetLanguageDetailsById(long lagLevelId)
        {
            SqlParameter param = new SqlParameter("@lagLevel_Id", lagLevelId);

            var languageLevelDetails = _languageLevelRepository.ExecuteStoredProcedureList<LanguageLevel>(PROC_GET_LANGUAGE_LEVEL_DETAILS_BY_ID, param).FirstOrDefault();
            return languageLevelDetails;
        }
        public ReturnMsg LanguageLevel(long lagLevelId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@lagLevel_Id", lagLevelId);
                _languageLevelRepository.ExecuteStoredProcedure(PROC_DELETE_LANGUAGE_LEVEL_DETAILS_BY_ID, param);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }

        public IList<LanguageLevel> GetAllLanguageLevels()
        {
            var languageLevelList = _languageLevelRepository.ExecuteStoredProcedureList<LanguageLevel>(PROC_GET_ALL_LANGUAGE_LEVELS);
            return languageLevelList;
        }

    }
}
