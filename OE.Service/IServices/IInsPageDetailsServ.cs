using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IInsPageDetailsServ
    {

        #region "Get Function Definitions" 
        IEnumerable<InsPageDetails> GetInsPageDetails();
        GetInsPageDetailsByInsPageId GetInsPageDetailsByInsPageId(long insPageId);
        InsPageDetails GetInsPageDetailsById(long id);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertInsPageDetails(InsertInsPageDetails obj);
        void UpdateInsPageDetails(InsPageDetails insPageDetails);
        
        DeleteInsPages DeleteInsPageDetails(DeleteInsPages obj);
       
        #endregion "Insert Update Delete Function Definitions"

    }
}
