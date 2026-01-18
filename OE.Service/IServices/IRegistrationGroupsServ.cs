
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IRegistrationGroupsServ
    {
        #region "Get Function Definitions"
        RegistrationGroups GetRegistrationGroupById(long Id);
        IEnumerable<GetRegistrationGroups> GetRegistrationGroups(long institutionId, long regUserTypeId = 0);
        #endregion "Get Function Definitions"

        #region "Insert Update and Delete Function Definitions"
        //void InsertRegistrationGroup(RegistrationGroups registrationGroup);
        string InsertRegistrationGroup(InsertRegistrationGroup obj);
        void UpdateRegistrationGroup(RegistrationGroups registrationGroup);
        void DeleteRegistrationGroup(RegistrationGroups registrationGroup);
        #endregion "Insert Update and Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_RegistrationGroups> Dropdown_RegistrationGroups(long institutionId, long userRegistrationTypeId = 0);
        #endregion "Dropdown Function Definitions"
    }
}
