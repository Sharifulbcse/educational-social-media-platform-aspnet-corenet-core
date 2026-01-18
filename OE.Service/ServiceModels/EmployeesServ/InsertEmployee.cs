
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using OE.Service.CustomEntitiesServ;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class InsertEmployee
    {
        public Employees Employees { get; set; }
        public IFormFile EmployeeImagefile { get; set; }
        public OE_Users OeUsers { get; set; }

        //[Extra Field from EmployeeDetails]
        public List<C_EmployeeDetails> EmployeeDetails { get; set; }

        //[Extra Fields from EmployeeDesignations]
        public long EmployeeTypeId { get; set; }
        public long EmployeeCategoryTypeId { get; set; }

        //[Extra Fields]
        public string Message { get; set; }
        public long CurrentInstitutionId { get; set; }
    }
}
