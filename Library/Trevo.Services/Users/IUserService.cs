using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.User;

namespace Trevo.Services.Users
{
    public partial interface IUserService
    {
        ReturnMsg InsertUserDetails(TrevoUsers details);

        ReturnMsg UpdateUserDetails(TrevoUsers details);
        TrevoUsers GetUserDetailsById(long userId);
        ReturnMsg DeleteUserDetails(long userId);
        IList<TrevoUsers> GetAllUsers();
        TrevoUsers GetUserDetailsByEmail(string email);
        TrevoUsers GetUserDetailsByEmailAndPassHash(string email, string passwordHash);
        List<TrevoUsers> GetAllUsersWithNativeLangId(long nativeLangId);
        List<UserWithCountryIcon> GetAllUsersWithCountryFlag();
        TrevoUsers GetUserDetailsByExternalAuthId(string id);
        ReturnMsg UpdateUserSelfIntroduction(string introduction, long userId);
        ReturnMsg UpdateUserName(string name, long userId);
        ReturnMsg UpdateUserAddress(string address, long userId);
        ReturnMsg UpdateUserProfileImage(string imagePath, long userId);
    }
}
