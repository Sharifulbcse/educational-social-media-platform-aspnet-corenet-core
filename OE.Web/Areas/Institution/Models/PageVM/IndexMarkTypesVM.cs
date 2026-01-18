
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexMarkTypesVM
    {
        public string InstitutionName { get; set; }
        public MarkTypesListVM MarkTypesList { get; set; }
        public List<MarkTypesListVM> _MarkTypesList { get; set; }
       
    }
}
