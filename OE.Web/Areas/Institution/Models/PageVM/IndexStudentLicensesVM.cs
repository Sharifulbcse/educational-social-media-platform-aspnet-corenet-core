using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexStudentLicensesVM
    {        
        public string InstitutionName { get; set; }
        public List<StudentsListVM> StudentsListVM { get; set; }       

        //[NOTE: Extra Fields]
        public long SelectedClassId { get; set; }
        public int CurrentYear { get; set; }
        public int SelectedFilterSearchId { get; set; }
        public string ErrorMessage { get; set; }
    }
}

