using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexRegistrationItemVM
    {
        public string InstitutionName { get; set; }       
        public long EmployeeTypeId { get; set; }
        public long EmployeeCategoryTypeId { get; set; }

        public StudentsListVM Students { get; set; }
        public EmployeeListVM Employees { get; set; }
        public RegistrationItemListVM RegistrationItemList { get; set; }
        public COM_RegistrationUserTypesVM COM_RegistrationUserTypesVM { get; set; }       

        public List<RegistrationItemListVM> _RegistrationItemList { get; set; }
        public List<RegistrationGroupListVM> _RegistrationGroupList { get; set; }        
        public List<StudentsDetailsListVM> studentsDetailsLists { get; set; } 
        public List<EmployeesDetailsListVM> EmpDetailsList { get; set; }

        //[NOTE: Extra field from StudentPromotion]
        public long RollNo { get; set; }

        //[NOTE: for dropdown]
        public SelectList _regITypes { get; set; }
        public SelectList _regGroups { get; set; }        
        public SelectList _UserList { get; set; }
        public SelectList _ClassList { get; set; }
        public SelectList _GenderList { get; set; }
        public SelectList _regUserTypeList { get; set; }
        public SelectList _EmpType { get; set; }
        public SelectList _EmpCategoryType { get; set; }    
    }
}
