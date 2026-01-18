
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexSubjectTypesVM
    {
        public string InstitutionName { get; set; }
        public SubjectTypesListVM SubjectTypesList { get; set; }
        public List<SubjectTypesListVM> _SubjectTypesList { get; set; }       
    }
}
