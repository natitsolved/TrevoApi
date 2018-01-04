using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.Language;

namespace Trevo.Services.Language
{
  public partial  interface ILanguageLevelService
    {
        bool InsertLanguageLevelDetails(LanguageLevel details);
        bool UpdateLanguageLevelDetails(LanguageLevel details);
        LanguageLevel GetLanguageDetailsById(long lagLevelId);
        ReturnMsg LanguageLevel(long lagLevelId);
        IList<LanguageLevel> GetAllLanguageLevels();
    }
}
