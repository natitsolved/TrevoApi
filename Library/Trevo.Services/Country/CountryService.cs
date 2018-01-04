using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using System.Linq;

namespace Trevo.Services.Country
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Core.Model.Country.Country> _countryRepository;

        private const string PROC_INSERT_COUNTRY = "spl_InsertCountryDetails @name,@flag_icon,@imagePath";
        private const string PROC_UPDATE_COUNTRY = "spl_UpdateCountryDetails @name,@flag_icon,@country_Id,@imagePath";
        private const string PROC_GET_ALL_COUNTRY = "spl_GetAllCountryDetails";
        private const string PROC_GET_COUNTRY_BY_ID = "spl_GetCountryDetailsById @country_Id";
        private const string PROC_DELETE_COUNTRY_BY_ID = "spl_DeleteCountryById @countryId";
        public CountryService(IRepository<Core.Model.Country.Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public bool InsertCountryDetails(Core.Model.Country.Country details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@flag_icon",Value = details.Flag_Icon,SqlDbType=SqlDbType.VarChar},
     new SqlParameter() {ParameterName = "@imagePath",Value = details.ImagePath,SqlDbType=SqlDbType.VarChar},
};

            object[] parameters = sp.ToArray();
            var users = _countryRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_COUNTRY, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCountryDetails(Core.Model.Country.Country details)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@country_Id",Value= details.Country_Id,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@name",Value = details.Name,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@flag_icon", Value = details.Flag_Icon,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@imagePath",Value = details.ImagePath,SqlDbType=SqlDbType.VarChar},
};

            object[] parameters = sp.ToArray();
            var users = _countryRepository.ExecuteStoredProcedureList<AuthUser>(PROC_UPDATE_COUNTRY, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IList<Core.Model.Country.Country> GetAllCountry()
        {
            var questionList = _countryRepository.ExecuteStoredProcedureList<Core.Model.Country.Country>(PROC_GET_ALL_COUNTRY);
            return questionList;
        }
        public Core.Model.Country.Country GetCountryDetailsById(long countryId)
        {
            SqlParameter param = new SqlParameter("@country_Id", countryId);

            var countryDetails = _countryRepository.ExecuteStoredProcedureList<Core.Model.Country.Country>(PROC_GET_COUNTRY_BY_ID, param).FirstOrDefault();
            return countryDetails;
        }

        public ReturnMsg DeleteCountry(long countryId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@countryId", countryId);
                _countryRepository.ExecuteStoredProcedure(PROC_DELETE_COUNTRY_BY_ID, param);
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
