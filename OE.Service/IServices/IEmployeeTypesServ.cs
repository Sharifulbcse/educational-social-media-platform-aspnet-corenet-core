
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IEmployeeTypesServ
    {
        #region "Get Function Definitions"
        EmployeeTypes GetEmployeeTypeById(Int64 id);
        IEnumerable<EmployeeTypes> GetEmployeeTypes(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertEmployeeTypes(InsertEmployeeTypes obj);
        void UpdateEmployeeTypes(EmployeeTypes employeeTypes);
        DeleteEmployeeTypes DeleteEmployeeTypes(DeleteEmployeeTypes obj);
        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"        
        IEnumerable<dropdown_EmployeeTypes> Dropdown_EmployeeTypes(long institutionId);
        #endregion "Dropdown Function Definitions"
    }
}
