
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexOE_UsersVM
    {        
        public List<OE_UsersListVM> OEUsersListVMs { get; set; }        
        public OE_UsersListVM users { get; set; }
        public SelectList _ddlUserlist { get; set; }
    }
}
