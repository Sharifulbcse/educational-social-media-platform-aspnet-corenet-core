using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;
namespace OE.Web.Areas.Institution.Models
{
    public class IndexOELicensesVM
    {
        public OELicensesListVM Licenses { get; set; }
        public Int64 userId { get; set; }
        public List<OELicensesListVM> _Licenses { get; set; }
        public SelectList _Institutions { get; set; }
        public SelectList _Users { get; set; }
    }
}
