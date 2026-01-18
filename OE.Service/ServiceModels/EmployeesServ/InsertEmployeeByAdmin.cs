using Microsoft.AspNetCore.Http;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class InsertEmployeeByAdmin
    {
        public Employees Employees { get; set; }
        public IFormFile EmployeeImagefile { get; set; }
        public OE_Users OeUsers { get; set; }

        //[Extra Fields from EmployeeDesignations]
        public long EmployeeTypeId { get; set; }
        public long EmployeeCategoryTypeId { get; set; }

        //[Extra Fields]
        public string Message { get; set; }
        public long CurrentInstitutionId { get; set; }
    }
}
