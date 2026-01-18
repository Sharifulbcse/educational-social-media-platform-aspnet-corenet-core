using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexOE_InstitutionsVM
    {
        public List<OE_InstitutionsListVM> _OE_InstitutionsList { get; set; }
        public IEnumerable<Int64> licenses { get; set; }
        public SelectList Countries { get; set; }        
        public OE_InstitutionsListVM OE_Institution { get; set; }
        
        //[NOTE: Extra fields]
        public IFormFile logo { get; set; }
        public IFormFile favicon { get; set; }

    }
}
