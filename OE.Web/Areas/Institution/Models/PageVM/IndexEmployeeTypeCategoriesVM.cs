using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexEmployeeTypeCategoriesVM
    {
        public string InstitutionName { get; set; }
        public EmployeeTypeCategoriesListVM EmpTypeCategoryList { get; set; }
        public List<EmployeeTypeCategoriesListVM> _EmpTypeCategoryList { get; set; }
        public SelectList _employeeType { get; set; }
    }
}

