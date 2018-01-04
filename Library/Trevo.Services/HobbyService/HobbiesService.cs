using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.Hobbies;
using Trevo.Core.Model.User;

namespace Trevo.Services.HobbyService
{
    public class HobbiesService : IHobbiesService
    {
        private readonly IRepository<HobbiesDetails> _hobbiesRepository;

        private const string PROC_INSERT_HOBBIES = "spl_InsertHobbies @name";
        private const string PROC_GET_ALL_HOBBIES = "spl_GetAllHobbies";
        private const string PROC_GET_HOBBIES_DETAILS_BY_ID = "spl_GetHobbiesDetailsById @hobbiesId";
        private const string PROC_INSERT_USER_HOBBIES = "spl_InsertUsersHobbiesDetails @user_id,@hobbiesid";
        private const string PROC_USER_HOBBIES_DETAILS_BY_USER_ID = "spl_GetAllUserHobbiesByUserId @user_id";
        private const string PROC_DELETE_USER_HOBBIES_BY_USER_ID = "spl_DeleteUserHobbiesByUserId @user_Id";
        public HobbiesService(IRepository<HobbiesDetails> hobbiesRepository)
        {
            _hobbiesRepository = hobbiesRepository;
        }

        public bool InsertHobbiesDetails(HobbiesDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.NChar},
};

            object[] parameters = sp.ToArray();
            var users = _hobbiesRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_HOBBIES, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<HobbiesDetails> GetAllHobbies()
        {
            var hobbiesList = _hobbiesRepository.ExecuteStoredProcedureList<HobbiesDetails>(PROC_GET_ALL_HOBBIES).ToList();
            return hobbiesList;
        }

        public HobbiesDetails GetHobbiesDetailsById(long id)
        {
            SqlParameter param = new SqlParameter("@hobbiesId", id);

            var hobbiesDetails = _hobbiesRepository.ExecuteStoredProcedureList<HobbiesDetails>(PROC_GET_HOBBIES_DETAILS_BY_ID, param).FirstOrDefault();
            return hobbiesDetails;
        }

        public bool InsertUserHobbiesDetails(UserHobbiesDetails details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@user_id",Value= details.User_Id,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@hobbiesid",Value= details.HobbiesId,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var users = _hobbiesRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_USER_HOBBIES, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<UserHobbiesDetails> GetUserHobbiesByUserId(long id)
        {
            SqlParameter param = new SqlParameter("@user_id", id);

            var hobbiesList = _hobbiesRepository.ExecuteStoredProcedureList<UserHobbiesDetails>(PROC_USER_HOBBIES_DETAILS_BY_USER_ID, param).ToList();
            return hobbiesList;
        }

        public ReturnMsg DeleteUserHobbiesByUserId(long id)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@user_Id", id);
                _hobbiesRepository.ExecuteStoredProcedure(PROC_DELETE_USER_HOBBIES_BY_USER_ID, param);
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
