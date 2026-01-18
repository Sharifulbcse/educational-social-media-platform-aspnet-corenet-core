
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; //[NOTE: for file]
using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IEmployeeDetailsServ
    {
        #region "Get Function Definitions"
        IEnumerable<EmployeeDetails> GetEmployeeDetails();
        EmployeeDetails GetEmployeeDetailsById(long id);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        void InsertEmployeeDetails(EmployeeDetails employeeDetails, IFormFile file, string webRoot, long? regIemTypeId);
        void UpdateEmployeeDetails(EmployeeDetails employeeDetails, IFormFile file, string webRoot, long? regIemTypeId);
        string UpdateEmployeeDetailsByAdmin(UpdateEmployeeDetailsByAdmin emp);

        void DeleteEmployeeDetails(EmployeeDetails employeeDetails);
        void DeleteStaticFile(EmployeeDetails emp, string rootPath, long? regItemTypeId);

        #endregion "Insert Update Delete Function Definitions"        
    }
}


