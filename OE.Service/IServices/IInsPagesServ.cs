
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IInsPagesServ
    {

        #region "Get Function Definitions" 
        IEnumerable<InsPages> GetInsPages();
        InsPages GetInsPagesById(long Id);
        #endregion "Get Function Definitions"

        #region "Insert Update DeleteFunction Definitions"
        void InsertInsPages(InsertInsPages obj, string WebRoot);

        void UpdateInsPages(InsPages insPages, IFormFile fle, string webRootPath);
        void DeleteInsPages(InsPages insPages, string webRootPath1, string webRootPath2);
        #endregion "Insert Update Delete Function Definitions"

    }
}
