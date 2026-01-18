
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface ICountriesServ
    {
        #region "Get Function Definitions"
        IEnumerable<Countries> GetCountries();
        Countries GetCountriesById(Int64 id);
        #endregion "Get Function Definitions"

        #region "Insert_Update_Delete Function Definitions"
        void InsertCountries(Countries countries);
        void UpdateCountries(Countries countries);
        void DeleteCountries(Countries countries);
        #endregion "Insert_Update_Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_Countries> Dropdown_Countries();
        #endregion "Dropdown Function Definitions"
    }
}
