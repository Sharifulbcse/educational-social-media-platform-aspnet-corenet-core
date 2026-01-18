
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexRegistrationFormByAdminVM
    {
        public string InstitutionName { get; set; }
        public EmployeeListVM Employees { get; set; }

        //[NOTE:Extra Fields from EmployeeDesignations]
        public long EmployeeTypeId { get; set; }
        public long EmployeeCategoryTypeId { get; set; }
    }
}
