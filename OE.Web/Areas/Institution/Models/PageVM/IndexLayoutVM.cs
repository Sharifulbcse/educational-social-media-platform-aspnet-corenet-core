
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexLayoutVM
    {
        public OE_UserAuthenticationsListVM menuActive { get; set; }
        public List<OE_UserAuthenticationsListVM> OE_UserAuthenticationsList { get; set; }
    } 
}
