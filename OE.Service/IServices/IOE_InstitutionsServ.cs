using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IOE_InstitutionsServ
    {
        #region "Get Function Definitions"      
        MyInstitutionDetails MyInstitutionDetails(Int64 insId);        
        OE_Institutions GetOE_InstitutionsById(Int64 id);
        OE_Institutions GetLicensesDetails();

        IEnumerable<OE_Institutions> GetOE_Institutions();       
        IEnumerable<GetInstitutionAndCountries> GetInstitutionAndCountries();        
        IEnumerable<Int64> GetLicenses();        
        IEnumerable<OE_Institutions> GetOE_InstitutionsByKeyWord(string keyword);
        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        void InsertOE_Institutions(InsertOE_Institution obj, string WebRoot);

        void UpdateOE_Institutions(OE_Institutions oE_Institutions,IFormFile logo, IFormFile favicon, string WebRoot);
        void UpdateOE_InstitutionsByAdmin(OE_Institutions oE_Institutions);

        void DeleteOE_Institutions(OE_Institutions oE_Institutions, string webRootPath);

        #endregion "Insert, Update and Delete Function Definitions"


        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_Institutions> Dropdown_Institutions();
        #endregion "Dropdown Function Definitions"    
    }
}
