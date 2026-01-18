using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IMarkTypesServ
    {

        #region "Get Function Definitions" 
        MarkTypes GetMarkTypesById(long Id);
        IEnumerable<MarkTypes> GetMarkTypes(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertMarkTypes(InsertMarkTypes obj);
        void UpdateMarkTypes(MarkTypes markTypes);
        DeleteMarkTypes DeleteMarkTypes(DeleteMarkTypes obj);
        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"        
        IEnumerable<dropdown_MarkTypes> Dropdown_MarkTypes(long institutionId);
        #endregion "Dropdown Function Definitions"

    }
}
