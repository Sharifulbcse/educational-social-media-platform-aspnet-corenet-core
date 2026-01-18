using System;
using Microsoft.AspNetCore.Http;//[NOTE: for 'IFormFile']
namespace OE.Service
{
    public interface ICommonFunctionsServ
    {
        #region "OE_UsersServ Function Definitions"
        Boolean Function_OEUsers_IsUserAsEmployee(string userLoginId);
        bool Function_OeUserHasUserLoginId(string userLoginId);
        #endregion "OE_UsersServ Function Definitions"
        
        #region "EmployeesServ Function Definitions"
        bool Function_IsEmployeeHasOeId(long oEId, long instituteId);
        #endregion "EmployeesServ Function Definitions"

        #region "StudentsServ Function Definitions"
        bool Function_IsStudentHasOeId(long oEId, long instituteId);
        bool Function_IsStudentHasExistingRollNo(long classId, long year, long rollNo);
        #endregion "StudentsServ Function Definitions"


    }
}
