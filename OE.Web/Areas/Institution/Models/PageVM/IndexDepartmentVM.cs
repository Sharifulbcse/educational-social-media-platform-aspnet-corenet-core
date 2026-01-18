
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexDepartmentVM
    {
        public string InstitutionName { get; set; }
        public DepartmentListVM DepartmentList { get; set; }
        public List<DepartmentListVM> _DepartmentList { get; set; }

    }
}
