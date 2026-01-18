using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexOEUserAuthenticationsVM
    {
        public List<OE_UserAuthenticationsListVM> OEUserAuthenticationsListVM { get; set; }
        public SelectList _Actors { get; set; }
        public SelectList _Users { get; set; }
    }
}
