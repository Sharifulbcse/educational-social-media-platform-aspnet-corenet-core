using Microsoft.AspNetCore.Http;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class UpdateEmployees
    {
        public Employees Employees { get; set; }
        public EmployeeDetails EmployeeDetails { get; set; }
        public EmployeeDesignations Designations { get; set; }
        public OE_UserAuthentications OE_UserAuthentications { get; set; }

        //[NOTE: Extra]
        public bool EnableAuthentic { get; set; }
        public IFormFile EmployeeDetailsStaticFile { get; set; }
        public IFormFile EmployeesStaticFile { get; set; }
        public long RegistrationItemTypeId { get; set; }
        public long SelectedUserId { get; set; }
        public long CurrentInstituteId { get; set; }
        public string Message { get; set; }
        public string SelectedUserLoginId { get; set; }
    }
}

