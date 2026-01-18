using System.Collections.Generic;
using OE.Data;

namespace OE.Service
{
    public interface ICOM_RegistrationUserTypesServ
    {

        #region "Get Function Definitions"
        COM_RegistrationUserTypes GetRegistrationUserTypeById(long id);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"

        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<Dropdown_COM_RegistrationUserTypes> Dropdown_COM_RegistrationUserTypes();
        #endregion "Dropdown Function Definitions"

    }
}
