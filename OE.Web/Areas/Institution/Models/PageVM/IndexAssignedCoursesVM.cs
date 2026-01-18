
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAssignedCoursesVM
    {
        public string InstitutionName { get; set; }
        public List<AssignedCoursesListVM> courseVM { get; set; }

        //[NOTE: for dropdown]
        public SelectList ddlClass { get; set; }
        public SelectList ddlSection { get; set; }
        public SelectList ddlSubject { get; set; }        
    }
}
