using System;
using OE.Data;
using System.Collections.Generic;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IEmployeeTypeCategoriesServ
    {

        #region "Get Function Definitions"        
        IEnumerable<EmployeeTypeCategories> GetEmployeeTypeCategories(long institutionId);
        EmployeeTypeCategories GetEmployeeTypeCategoriesById(Int64 id);
        IEnumerable<GetEmployeeTypeCategories> GetEmployeeType(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertEmployeeTypeCategories(GetEmployeeTypeCategories obj);
        void UpdateEmployeeTypeCategories(EmployeeTypeCategories employeeTypeCategories);
        DeleteEmployeeTypeCategories DeleteEmployeeTypeCategories(DeleteEmployeeTypeCategories obj);
        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_EmployeeTypeCategories> Dropdown_EmployeeTypeCategories(long instituteId, long ddlEmpTypeId);
        #endregion "Dropdown Function Definitions"

    }
}

