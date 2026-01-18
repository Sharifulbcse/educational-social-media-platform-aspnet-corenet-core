
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexEmployeeTypeVM
    {
        public string InstitutionName { get; set; }
        public List<EmployeeTypeListVM> _EmployeeTypeList { get; set; }
        public EmployeeTypeListVM EmployeeTypeList { get; set; }
    }
}
