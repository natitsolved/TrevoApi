using Trevo.Core.Model;
using Trevo.Core.Model.UserTransliteration;

namespace Trevo.Services.TransliterationService
{
    public  interface IUserTransliterationService
    {
        ReturnMsg UpdateTransliterationByUserId(UserTransliterationDetails details);
        bool InsertTransliterationDetails(UserTransliterationDetails details);
        UserTransliterationDetails GetTransliterationDetailsByUserId(long id);
    }
}
