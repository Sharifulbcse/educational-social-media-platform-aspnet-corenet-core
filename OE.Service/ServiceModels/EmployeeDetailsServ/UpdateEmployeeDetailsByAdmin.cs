
using Microsoft.AspNetCore.Http;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class UpdateEmployeeDetailsByAdmin
    {
        public Employees Employees { get; set; }
        public OE_Users OeUsers { get; set; }
        public EmployeeDesignations Designations { get; set; }
        //[NOTE: Extra fields]       
        public IFormFile ProfileImage { get; set; }
        public string WebRootPath { get; set; }
       
        public string Message { get; set; }
        public long CurrentInstitutionId { get; set; }
    }
}
