using System.Collections.Generic;
using Trevo.Core.Model;

namespace Trevo.Services.Country
{
    public partial interface ICountryService
    {
        IList<Core.Model.Country.Country> GetAllCountry();
        Core.Model.Country.Country GetCountryDetailsById(long countryId);
        ReturnMsg DeleteCountry(long countryId);
        bool InsertCountryDetails(Core.Model.Country.Country details);
        bool UpdateCountryDetails(Core.Model.Country.Country details);
    }
}
