using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IOE_LicensesServ
    {
        #region "Get Function Definitions"
        IEnumerable<GetLicensesList> GetLicensesList();
       
        IEnumerable<GetLicenses> GetLicenses(Int64 institutionTypeId);

        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"  
        void InsertLicenses(InsertLicenses obj, IFormFile fle, string webRootPath);
        
        void UpdateLicenses(OE_Licenses license);
        void DeleteLicenses(OE_Licenses license);
        #endregion "Insert, Update and Delete Function Definitions"

    }
}
