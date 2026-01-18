using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexOE_InstitutionsByAdminVM
    {
        public OE_InstitutionsListVM OE_Institution { get; set; }
        public OELicensesListVM licenses { get; set; }
    }
}
