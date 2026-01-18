
using System.Collections.Generic;

using Microsoft.AspNetCore.Http; //[NOTE: for IFormFile]

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IEmployeesServ
    {

        #region "Get Function Definitions"
        Employees GetEmployeeByTeacher(long institutionId, long userId);

        Employees EmployeeById(long id, long instituteId, long userId);
        GetEmployeeDetails GetEmployeeDetails(long institutionId, long empId);
        GetEmployeeListByAdmin GetEmployeeListByAdmin(long instituteId, bool employeeStatus);
        GetEmployeeListByRegister GetEmployeeListByRegister(long instituteId);
        GetEmployeeDetailsByAdmin GetEmployeeDetailsByAdmin(long institutionId, long empId);

        IEnumerable<Employees> GetEmployees(long instituteId);       
        IEnumerable<GetTeacherLicenses> GetTeacherLicenses(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert update and delete Function Definitions"        
        string InsertEmployee(InsertEmployee emp, string webRootPath);
        string InsertEmployeeByAdmin(InsertEmployeeByAdmin emp, string webRootPath);
        string InsertTeacherLicense(InsertTeacherLicenses teacherLicenses);
               
        string UpdateEmployees(UpdateEmployees obj, string webRootPath);
        string UpdateTeacherLicense(UpdateTeacherLicenses staffLicenses);

        DeleteEmployees DeleteEmployees(DeleteEmployees obj);
        void DeleteStaticFile(Employees emp, string rootPath);
        DeleteTeacherLicenses DeleteTeacherLicense(DeleteTeacherLicenses obj);
        void MakeArchive(MakeArchiveEmployees obj);
        #endregion "Insert update and delete Function Definitions"       

        #region "Dropdown Function Definitions"        
        IEnumerable<dropdown_Employees> Dropdown_Employees(long institutionId);
        IEnumerable<dropdown_EmployeeWithAssignTeacher> dropdown_EmployeeWithAssignTeacher(long institutionId, long year, long classId, long subjectId);
        #endregion "Dropdown Function Definitions"
    }
}
