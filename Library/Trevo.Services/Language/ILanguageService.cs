using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.Language;

namespace Trevo.Services.Language
{
    public partial  interface ILanguageService
    {

        bool InsertLanguageDetails(LanguageDetails details);
        bool UpdateLanguageDetails(LanguageDetails details);
        LanguageDetails GetLanguageDetailsById(long languageId);
        ReturnMsg DeleteLanguage(long languageId);
        IList<LanguageDetails> GetAllLangugaes();
    }
}
