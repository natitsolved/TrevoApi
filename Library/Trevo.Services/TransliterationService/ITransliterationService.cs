using System.Collections.Generic;
using Trevo.Core.Model.UserTransliteration;

namespace Trevo.Services.TransliterationService
{
    public interface ITransliterationService
    {
        bool InsertTransliteration(TransliterationDetails details);
        List<TransliterationDetailsWithImage> GetTransliterationDetailsByUserId(long id);
    }
}
