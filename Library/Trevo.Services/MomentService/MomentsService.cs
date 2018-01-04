using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.MomentDetails;
using System.Linq;
namespace Trevo.Services.MomentService
{
    public class MomentsService :IMomentsService
    {
        private readonly IRepository<Moments> _momentsRepository;

        private const string PROC_INSERT_MOMENTS_DETAILS = "spl_InsertMomentDetails @message,@parentId,@posterUserId,@postingTime,@userUploadedId";
        private const string PROC_UPDATE_MOMENTS_DETAILS = "spl_UpdateMomentDetails @momentId,@parentId,@posterUserId,@message,@postingTime,@userUploadedId";
        private const string PROC_GET_ALL_MOMENTS_BY_ID = "spl_GetAllMomentDetailsById @momentId";
        private const string PROC_DELETE_MOMENTS_BY_ID = "spl_DeleteMomentDetailsById @momentId";
        private const string PROC_GET_ALL_MOMENTS = "spl_GetAllMoments";
        private const string PROC_GET_MOMENTS_LIST_BY_USER_ID = "spl_getAllMomentsListByUserId @user_Id";
        private const string PROC_GET_MOMENTS_LIST_BY_USER_ID_FOR_EXCHANGE = "spl_getAllMomentsListByUserIdForExchange @nativeLangId,@learningLangId";
        private const string PROC_GET_MOMENT_LIST_BY_PARENT_ID = "spl_GetMomentsListByParentId @parentId";
        private const string PROC_GET_MOMENT_LIST_BY_NATIVE_LEARN_LANG = "spl_GetAllMomentsByUserNativeAndLearnLang @nativeLang,@learningLang";
        private const string PROC_GET_MOMENT_LIST_BY_FOLLOWER_USERID = "spl_GetAllMomentsByUserFollwingId @followerUserId";
        public MomentsService(IRepository<Moments> momentsRepository)
        {
            _momentsRepository = momentsRepository;
        }

        public bool InsertMomentDetails(Moments details)
        {


            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@message",Value= details.Message,SqlDbType=SqlDbType.VarChar},
      new SqlParameter() {ParameterName = "@parentId",Value= details.ParentId,SqlDbType=SqlDbType.BigInt},
      new SqlParameter() {ParameterName = "@posterUserId",Value= details.PosterUserId,SqlDbType=SqlDbType.BigInt},
       new SqlParameter() {ParameterName = "@postingTime",Value= details.PostingTime,SqlDbType=SqlDbType.VarChar},
       new SqlParameter() {ParameterName = "@userUploadedId",Value= details.UserUploadedId,SqlDbType=SqlDbType.BigInt},
};

            object[] parameters = sp.ToArray();
            var users = _momentsRepository.ExecuteStoredProcedureList<AuthUser>(PROC_INSERT_MOMENTS_DETAILS, parameters);
            if (users != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ReturnMsg UpdateMomentDetails(Moments details)
        {
            ReturnMsg msg = new ReturnMsg();

            try
            {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@momentId",Value= details.MomentId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@parentId",Value= details.ParentId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@posterUserId",Value= details.PosterUserId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@message ",Value= details.Message,SqlDbType=SqlDbType.VarChar},
     new SqlParameter() {ParameterName = "@postingTime",Value= details.PostingTime,SqlDbType=SqlDbType.VarChar},
     new SqlParameter() {ParameterName = "@userUploadedId",Value= details.UserUploadedId,SqlDbType=SqlDbType.BigInt},
};

                object[] parameters = sp.ToArray();
                _momentsRepository.ExecuteStoredProcedure(PROC_UPDATE_MOMENTS_DETAILS, parameters);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }


            return msg;
        }

        public Moments GetMomentDetailsById(long momentId)
        {
            SqlParameter param = new SqlParameter("@momentId", momentId);
            var momentDetails = _momentsRepository.ExecuteStoredProcedureList<Moments>(PROC_GET_ALL_MOMENTS_BY_ID, param).FirstOrDefault();
            return momentDetails;
        }

        public ReturnMsg DeleteMoments(long momentId)
        {
            ReturnMsg msg = new ReturnMsg();
            try
            {
                SqlParameter param = new SqlParameter("@momentId", momentId);
                _momentsRepository.ExecuteStoredProcedure(PROC_DELETE_MOMENTS_BY_ID, param);
                msg.IsSuccess = true;
            }
            catch (Exception e)
            {
                msg.IsSuccess = false;
                msg.Message = e.Message;
            }
            return msg;
        }

        public List<MomentDetailsWithImage> GetAllMoments()
        {
            var momentList = _momentsRepository.ExecuteStoredProcedureList<MomentDetailsWithImage>(PROC_GET_ALL_MOMENTS).ToList();
            return momentList;
        }


        public List<MomentDetailsWithImage> GetAllMomentsLIstByParentId(long id)
        {
            SqlParameter param = new SqlParameter("@parentId", id);

            var momentList = _momentsRepository.ExecuteStoredProcedureList<MomentDetailsWithImage>(PROC_GET_MOMENT_LIST_BY_PARENT_ID, param).ToList();
            return momentList;
        }

        public List<MomentDetailsWithImage> GetMomentsListByUserId(long id)
        {
            SqlParameter param = new SqlParameter("@user_Id", id);

            var momentList = _momentsRepository.ExecuteStoredProcedureList<MomentDetailsWithImage>(PROC_GET_MOMENTS_LIST_BY_USER_ID, param).ToList();
            return momentList;
        }

        public List<MomentDetailsWithImage> GetMomentListByNativeLearnLang(string nativeLang,string learningLang)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@nativeLang",Value= nativeLang,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@learningLang",Value= learningLang,SqlDbType=SqlDbType.VarChar},
};
            object[] parameters = sp.ToArray();
            var momentList = _momentsRepository.ExecuteStoredProcedureList<MomentDetailsWithImage>(PROC_GET_MOMENT_LIST_BY_NATIVE_LEARN_LANG, parameters).ToList();
            return momentList;
        }


        public List<MomentDetailsWithImage> GetMomentsListByFollowerUserId(long id)
        {
            SqlParameter param = new SqlParameter("@followerUserId", id);

            var momentList = _momentsRepository.ExecuteStoredProcedureList<MomentDetailsWithImage>(PROC_GET_MOMENT_LIST_BY_FOLLOWER_USERID, param).ToList();
            return momentList;
        }


        public List<MomentDetailsWithLang> GetMomentListByUserIdForExchange(long nativeId,long learningId)
        {
                List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@nativeLangId",Value= nativeId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@learningLangId",Value= learningId,SqlDbType=SqlDbType.BigInt},
};

                object[] parameters = sp.ToArray();
            var momentList = _momentsRepository.ExecuteStoredProcedureList<MomentDetailsWithLang>(PROC_GET_MOMENTS_LIST_BY_USER_ID_FOR_EXCHANGE, parameters).ToList();
            return momentList;
        }
    }
}
