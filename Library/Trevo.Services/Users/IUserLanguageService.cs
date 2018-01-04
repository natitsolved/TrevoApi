using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.User;

namespace Trevo.Services.Users
{
    public partial  interface IUserLanguageService
    {
        bool InsertUserLanguageDetails(UsersLanguage details);

        bool UpdateUserLanguageDetails(UsersLanguage details);
        UsersLanguage GetLanguageListByUserId(long userId);
        IList<UsersLanguage> GetAllUsersLanguages();
        ReturnMsg DeleteUserLanguage(long id);
    }

}
