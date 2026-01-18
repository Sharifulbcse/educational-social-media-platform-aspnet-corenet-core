
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering; //[NOTE: for SelectList]

namespace OE.Web.Areas.Institution.Models
{
    public class IndexStudentsVM
    {
        public string InstitutionName { get; set; }

        public List<StudentsListVM> _Students { get; set; }                
        public StudentsListVM Student { get; set; }

        public List<RegistrationGroupListVM> _RegistrationGroups { get; set; }
        public List<RegistrationItemListVM> _RegistrationItems { get; set; }
        public List<StudentsDetailsListVM> _StudentDetails { get; set; }

    }
}

