
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexClassesVM
    {
        public string InstitutionName { get; set; }
        public ClassesListVM classesVM { get; set; }
        public List<ClassesListVM> _classesVM { get; set; }        
        public SelectList _InstitutionsListVM { get; set; }
        
    }
}
