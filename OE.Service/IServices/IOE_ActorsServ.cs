using System;
using OE.Data;
using OE.Service.ServiceModels;
using System.Collections.Generic;

namespace OE.Service
{
    public interface IOE_ActorsServ
    {
        IEnumerable<OE_Actors> GetActors();

        #region "DropDown Function Definitions"        
        IEnumerable<dropdown_Actors> Dropdown_Actors(Int64 id);
        #endregion "DropDown Function Definitions"

    }
}
