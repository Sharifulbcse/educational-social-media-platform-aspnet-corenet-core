
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexOE_StaffsVM
    {
        public List<OE_StaffsListVM> _OE_StaffVM { get; set; }       
        public SelectList _OE_StaffTypeList { get; set; }
    }
}
