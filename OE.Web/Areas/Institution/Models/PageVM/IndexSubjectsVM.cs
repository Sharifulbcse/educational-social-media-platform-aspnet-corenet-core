
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexSubjectsVM
    {
        public string InstitutionName { get; set; }
        public SubjectsListVM subjectsVM { get; set; }
        public List<SubjectsListVM> _subjectsVM { get; set; }
        
        //[NOTE: Extra Fields]
        public long SelectedClassId { get; set; }
    }
}
