using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexRegistrationGroupVM
    {
        public string InstitutionName { get; set; }
        public RegistrationGroupListVM RegistrationGroupList { get; set; }
        public List<RegistrationGroupListVM> _RegistrationGroupList { get; set; }
        public SelectList _regUsr { get; set; }
        public COM_RegistrationUserTypesVM COM_RegistrationUserTypesVM { get; set; }
    }
}
