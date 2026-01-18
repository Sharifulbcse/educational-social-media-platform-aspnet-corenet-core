using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAssignedSectionVM
    {
        public string InstitutionName { get; set; }
        public List<AssignedSectionListVM> AssignedSectionList { get; set; }
        public SelectList ddlClass { get; set; }
        
        //[NOTE: Extra Fields]
        public int CurrentSearchYear { get; set; }
       
    }
}
