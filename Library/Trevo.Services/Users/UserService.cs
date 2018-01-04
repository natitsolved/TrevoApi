using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Trevo.Core.Data;
using Trevo.Core.Model;
using Trevo.Core.Model.User;
using System.Linq;

namespace Trevo.Services.Users
{
    public partial class UserService : IUserService
    {
        private readonly IRepository<TrevoUsers> _userRepository;

        private const string PROC_INSERT_USER_DETAILS = "spl_InsertTrevoUsers";
        private const string PROC_UPDATE_USER_DETAILS = "spl_UpdateTrevoUsers @name,@email_Id,@password,@dob,@gender,@deviceId,@trevoId,@self_introduction,@countryId,@address,@qrCode,@interests,@travelDestinationCid,@lagLevelId,@userId,@isVerified,@imagePath,@createdTime,@passwordHash,@externalAuthType,@externalAuthUserId";
        private const string PROC_GET_USERDETAILS_BY_USERID = "spl_GetUserDetailsById @userId";
        private const string PROC_DELETE_USERDETAILS_BY_USERID = "spl_DeleteUserDetailsById @userId";
        private const string PROC_GET_ALL_USERS = "spl_GetAllUsers";
        private const string PROC_GET_USER_BY_EMAIL = "spl_GetUserByEmail @email";
        private const string PROC_GET_USER_BY_EMAIL_AND_PASSHASH = "spl_GetUserByEmailAndPassHash @email,@passwordHash";
        private const string PROC_GET_ALL_USERS_WITH_NATIVE_LANGID = "spl_GetAllUsersWithNativeLang @nativeLangId";
        private const string PROC_GET_ALL_USERS_WITH_COUNTRY_FLAG_ICON = "spl_GetAllUsersWithCountryFlag";
        private const string PROC_CHECK_IF_USER_EXISTS_BY_EXTERNAL_AUTH_ID = "spl_CheckIfUserExistByExternalAuthId @externalAuthUserId";
        private const string PROC_UPDATE_USER_SELF_INTRODUCTION = "spl_UpdateUserSelfIntroduction @self_Introduction,@user_Id";
        private const string PROC_UPDATE_USER_NAME = "spl_UpdateUserName @name,@user_Id";
        private const string PROC_UPDATE_USER_ADDRESS = "spl_UpdateUserAddress @address,@user_Id";
        private const string PROC_UPDATE_USER_PROFILE_IMAGE = "spl_UpdateUserProfileImage @imagePath,@user_Id";
        public UserService(IRepository<TrevoUsers> userRepository)
        {
            _userRepository = userRepository;
        }

        public ReturnMsg InsertUserDetails(TrevoUsers details)
        {
            ReturnMsg obj = new ReturnMsg();

            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@email_Id",Value= details.Email_Id,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@password",Value= details.Password,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@dob",Value= details.Dob,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@gender",Value= details.Gender,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@deviceId ",Value= details.DeviceId,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@trevoId",Value= details.TrevoId,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@self_introduction",Value= details.Self_Introduction,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@countryId ",Value= details.Country_Id ,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@address ",Value= details.Address,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@qrCode",Value= details.QR_Code,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@interests",Value= details.Interests,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@travelDestinationCid",Value= details.TravelDestination_CId,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@lagLevelId",Value= details.LagLevel_ID,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@passwordHash",Value= details.PasswordHash,SqlDbType=SqlDbType.VarChar},
     new SqlParameter() {ParameterName = "@isVerified",Value= details.IsVerified,SqlDbType=SqlDbType.Int},
      new SqlParameter() {ParameterName = "@imagePath",Value= details.ImagePath,SqlDbType=SqlDbType.VarChar},
       new SqlParameter() {ParameterName = "@createdTime",Value= details.CreatedTime,SqlDbType=SqlDbType.DateTime},
         new SqlParameter() {ParameterName = "@externalAuthType",Value= details.ExternalAuthType,SqlDbType=SqlDbType.VarChar},
           new SqlParameter() {ParameterName = "@externalAuthUserId",Value= details.ExternalAuthUserId,SqlDbType=SqlDbType.VarChar},
         new SqlParameter() {ParameterName = "@id",SqlDbType=SqlDbType.Int,Direction=ParameterDirection.Output},
};

            object[] parameters = sp.ToArray();
            List<object> pa = new List<object>();
            _userRepository.ExecuteStoredProcedureForOutParams(PROC_INSERT_USER_DETAILS, out pa, parameters);
            if (pa.Count > 0)
            {
                obj.IsSuccess = true;
                obj.Message = pa[0];
            }
            else
            {
                obj.IsSuccess = false;
            }
            return obj;
        }

        public ReturnMsg UpdateUserDetails(TrevoUsers details)
        {
            ReturnMsg msg = new ReturnMsg();

            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= details.Name,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@email_Id",Value= details.Email_Id,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@password",Value= details.Password,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@dob",Value= details.Dob,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@gender",Value= details.Gender,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@deviceId ",Value= details.DeviceId,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@trevoId",Value= details.TrevoId,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@self_introduction",Value= details.Self_Introduction,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@countryId ",Value= details.Country_Id ,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@address ",Value= details.Address,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@qrCode",Value= details.QR_Code,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@interests",Value= details.Interests,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@travelDestinationCid",Value= details.TravelDestination_CId,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@lagLevelId",Value= details.LagLevel_ID,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@userId",Value= details.User_Id,SqlDbType=SqlDbType.BigInt},
     new SqlParameter() {ParameterName = "@isVerified",Value= details.IsVerified,SqlDbType=SqlDbType.Int},
      new SqlParameter() {ParameterName = "@imagePath",Value= details.ImagePath,SqlDbType=SqlDbType.VarChar},
       new SqlParameter() {ParameterName = "@createdTime",Value= details.CreatedTime,SqlDbType=SqlDbType.DateTime},
           new SqlParameter() {ParameterName = "@passwordHash",Value= details.PasswordHash,SqlDbType=SqlDbType.VarChar},
            new SqlParameter() {ParameterName = "@externalAuthType",Value= details.ExternalAuthType,SqlDbType=SqlDbType.VarChar},
             new SqlParameter() {ParameterName = "@externalAuthUserId",Value= details.ExternalAuthUserId,SqlDbType=SqlDbType.VarChar},
};

                object[] parameters = sp.ToArray();
                _userRepository.ExecuteStoredProcedure(PROC_UPDATE_USER_DETAILS, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }


            return msg;
        }

        public TrevoUsers GetUserDetailsById(long userId)
        {
            SqlParameter param = new SqlParameter("@userId", userId);

            var userDetails = _userRepository.ExecuteStoredProcedureList<TrevoUsers>(PROC_GET_USERDETAILS_BY_USERID, param).FirstOrDefault();
            return userDetails;
        }
        public ReturnMsg DeleteUserDetails(long userId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@userId", userId);
                _userRepository.ExecuteStoredProcedure(PROC_DELETE_USERDETAILS_BY_USERID, param);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }

        public TrevoUsers GetUserDetailsByEmail(string email)
        {
            SqlParameter param = new SqlParameter("@email", email);

            var userDetails = _userRepository.ExecuteStoredProcedureList<TrevoUsers>(PROC_GET_USER_BY_EMAIL, param).FirstOrDefault();
            return userDetails;
        }
        public IList<TrevoUsers> GetAllUsers()
        {
            var userList = _userRepository.ExecuteStoredProcedureList<TrevoUsers>(PROC_GET_ALL_USERS);
            return userList;
        }

        public TrevoUsers GetUserDetailsByEmailAndPassHash(string email, string passwordHash)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@email",Value= email,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@passwordHash",Value= passwordHash,SqlDbType=SqlDbType.VarChar},
};
            object[] parameters = sp.ToArray();
            var userDetails = _userRepository.ExecuteStoredProcedureList<TrevoUsers>(PROC_GET_USER_BY_EMAIL_AND_PASSHASH, parameters).FirstOrDefault();
            return userDetails;
        }

        public List<TrevoUsers> GetAllUsersWithNativeLangId(long nativeLangId)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@nativeLangId",Value= nativeLangId,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var userDetails = _userRepository.ExecuteStoredProcedureList<TrevoUsers>(PROC_GET_ALL_USERS_WITH_NATIVE_LANGID, parameters).ToList();
            return userDetails;
        }
        public List<UserWithCountryIcon> GetAllUsersWithCountryFlag()
        {
            var userList = _userRepository.ExecuteStoredProcedureList<UserWithCountryIcon>(PROC_GET_ALL_USERS_WITH_COUNTRY_FLAG_ICON).ToList();
            return userList;
        }

        public TrevoUsers GetUserDetailsByExternalAuthId(string id)
        {
            SqlParameter param = new SqlParameter("@externalAuthUserId", id);

            var userDetails = _userRepository.ExecuteStoredProcedureList<TrevoUsers>(PROC_CHECK_IF_USER_EXISTS_BY_EXTERNAL_AUTH_ID, param).FirstOrDefault();
            return userDetails;
        }

        public ReturnMsg UpdateUserSelfIntroduction(string introduction,long userId)
        {
            ReturnMsg msg = new ReturnMsg();

            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@self_Introduction",Value= introduction,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@user_Id",Value= userId,SqlDbType=SqlDbType.BigInt},
};

                object[] parameters = sp.ToArray();
                _userRepository.ExecuteStoredProcedure(PROC_UPDATE_USER_SELF_INTRODUCTION, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }


            return msg;
        }
        public ReturnMsg UpdateUserName(string name, long userId)
        {
            ReturnMsg msg = new ReturnMsg();

            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@name",Value= name,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@user_Id",Value= userId,SqlDbType=SqlDbType.BigInt},
};

                object[] parameters = sp.ToArray();
                _userRepository.ExecuteStoredProcedure(PROC_UPDATE_USER_NAME, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }


            return msg;
        }
        public ReturnMsg UpdateUserAddress(string address, long userId)
        {
            ReturnMsg msg = new ReturnMsg();

            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@address",Value= address,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@user_Id",Value= userId,SqlDbType=SqlDbType.BigInt},
};

                object[] parameters = sp.ToArray();
                _userRepository.ExecuteStoredProcedure(PROC_UPDATE_USER_ADDRESS, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }


            return msg;
        }

        public ReturnMsg UpdateUserProfileImage(string imagePath, long userId)
        {
            ReturnMsg msg = new ReturnMsg();

            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@imagePath",Value= imagePath,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@user_Id",Value= userId,SqlDbType=SqlDbType.BigInt},
};

                object[] parameters = sp.ToArray();
                _userRepository.ExecuteStoredProcedure(PROC_UPDATE_USER_PROFILE_IMAGE, parameters);
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
