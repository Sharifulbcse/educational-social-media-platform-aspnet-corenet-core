using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexInstitutionLinksVM
    {
        public string InstitutionName { get; set; }
        public List<InstitutionLinksListVM> _InstitutionLinksList { get; set; }
        public InstitutionLinksListVM InstitutionLinks { get; set; }
        public IFormFile socialimage { get; set; }
    }
}
