
using System;
using OE.Data;
using System.Collections.Generic;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IRegistrationItemsServ
    {

        #region "Get Function Definitions"    
        RegistrationItems GetRegistrationItemById(long Id);
        IEnumerable<GetRegItems> GetRegItems(long institutionId, long regUserTypeId = 0);
        IEnumerable<GetRegItems> GetRegItemsWithDetails(long instituteId, long studentOrEmployeeId, long regUerType);

        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        //void InsertRegistrationItem(RegistrationItems registrationItem);
        string InsertRegistrationItem(InsertRegistrationItem obj);
        void UpdateRegistrationItem(RegistrationItems registrationItem);
        void DeleteRegistrationItem(RegistrationItems registrationItem);
        #endregion "Insert Update Delete Function Definitions"


    }
}
