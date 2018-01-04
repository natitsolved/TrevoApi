using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.Hobbies;
using Trevo.Core.Model.User;

namespace Trevo.Services.HobbyService
{
  public  interface IHobbiesService
    {
       bool InsertHobbiesDetails(HobbiesDetails details);
        List<HobbiesDetails> GetAllHobbies();
        HobbiesDetails GetHobbiesDetailsById(long id);
        bool InsertUserHobbiesDetails(UserHobbiesDetails details);
        List<UserHobbiesDetails> GetUserHobbiesByUserId(long id);
        ReturnMsg DeleteUserHobbiesByUserId(long id);
    }
}
