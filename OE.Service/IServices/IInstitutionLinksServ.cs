
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IInstitutionLinksServ
    {

        #region "Get Function Definitions" 
        InstitutionLinks GetInstitutionLinksById(long Id);
        IEnumerable<InstitutionLinks> GetInstitutionLinks(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertInstitutionLinks(InsertInstitutionLinks obj, string WebRoot);
        void UpdateInstitutionLinks(InstitutionLinks institutionLinks, IFormFile fle, string webRootPath);
        DeleteInstitutionLinks DeleteInstitutionLinks(DeleteInstitutionLinks obj, string webRootPath);
        #endregion "Insert Update Delete Function Definitions"


    }
}
