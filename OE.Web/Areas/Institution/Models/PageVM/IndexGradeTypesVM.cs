
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexGradeTypesVM
    {
        public string InstitutionName { get; set; }
        public GradeTypesListVM gradeTypesVM { get; set; }
        public List<GradeTypesListVM> _gradeTypesVM { get; set; }

        //[NOTE: Extra properties]
        public long SelectedClassId { get; set; }
    }
}
