using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trevo.Core.Data;
using Trevo.Core.DTO;
using Trevo.Core.Model;
using Trevo.Core.Model.UserUploadDetails;

namespace Trevo.Services.UserUploadService
{
    public partial  class UserUploadsService : IUserUploadsService
    {
        private readonly IRepository<UserUploads> _userUploadsRepository;

        private const string PROC_INSERT_USER_UPLOADS = "spl_InsertUserUploadDetails";
        private const string PROC_GET_USER_UPLOADS_BY_ID = "spl_GetAllUserUploadsById @userUploadedId";

        public UserUploadsService(IRepository<UserUploads> userUploadsRepository)
        {
            _userUploadsRepository = userUploadsRepository;
        }

        public ReturnMsg InsertUserUploadDetails(UserUploads details)
        {
            ReturnMsg obj = new ReturnMsg();

            List<SqlParameter> sp = new List<SqlParameter>()
{
    new SqlParameter() {ParameterName = "@userId",Value= details.UserId,SqlDbType=SqlDbType.BigInt},
    new SqlParameter() {ParameterName = "@uploadedImagePath",Value= details.UploadedImagePath,SqlDbType=SqlDbType.VarChar},
    new SqlParameter() {ParameterName = "@uploadedAudioPath",Value= details.UploadedAudioPath,SqlDbType=SqlDbType.VarChar},
     new SqlParameter() {ParameterName = "@id",SqlDbType=SqlDbType.Int,Direction=ParameterDirection.Output},
};

            object[] parameters = sp.ToArray();
            List<object> pa = new List<object>();
             _userUploadsRepository.ExecuteStoredProcedureForOutParams(PROC_INSERT_USER_UPLOADS,out pa, parameters);
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

        public UserUploads GetUserUploadsById(long id)
        {
            SqlParameter param = new SqlParameter("@userUploadedId", id);

            var list = _userUploadsRepository.ExecuteStoredProcedureList<UserUploads>(PROC_GET_USER_UPLOADS_BY_ID, param).FirstOrDefault();
            return list;
        }
    }
}
