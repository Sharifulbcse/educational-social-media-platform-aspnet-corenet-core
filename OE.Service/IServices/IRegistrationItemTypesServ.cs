
using System;
using OE.Data;
using System.Collections.Generic;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IRegistrationItemTypesServ
    {

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_RegistrationItemTypes> Dropdown_RegistrationItemTypes();
        #endregion "Dropdown Function Definitions"

    }
}
