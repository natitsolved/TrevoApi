using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.UserUploadDetails;

namespace Trevo.Services.UserUploadService
{
   public partial interface IUserUploadsService
    {
        ReturnMsg InsertUserUploadDetails(UserUploads details);
        UserUploads GetUserUploadsById(long id);
    }
}
