
using System;
using OE.Data;
using System.Collections.Generic;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IDepartmentsServ
    {

        #region "Get Function Definitions"        
        IEnumerable<Departments> GetDepartments(long institutionId);
        
        Departments GetDepartmentById(Int64 id);
        #endregion "Get Function Definitions"

        #region Insert_Update_Delete Function Definitions"
        string InsertDepartments(InsertDepartments obj);
        void UpdateDepartments(Departments departments);
        DeleteDepartments DeleteDepartments(DeleteDepartments obj);
        #endregion "Insert_Update_Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_InsCategories> Dropdown_Departments();
        #endregion "Dropdown Function Definitions"

    }
}
