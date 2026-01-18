
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace OE.Web.Areas.Institution.Models
{
    public class IndexEmployeeVM
    {
       
        public string InstitutionName { get; set; }

        public EmployeeListVM employees { get; set; }
        public EmployeeDesignationsListVM Designations { get; set; }

        public List<EmployeeListVM> employeelist { get; set; }
        public List<RegistrationGroupListVM> regGroup { get; set; }
        public List<RegistrationItemListVM> regItems { get; set; }
        public List<EmployeesDetailsListVM> regItemDetails { get; set; }

        //[NOTE: for Dropdown list]
        public SelectList ddlGender { get; set; }
        public SelectList _EmployeeTypeList { get; set; }
        public SelectList _DepartmentList { get; set; }
        public SelectList DdlOeUsers { get; set; }

        //[NOTE:Extra field]
        public bool SelectedEmployee { get; set; }


    }
}
