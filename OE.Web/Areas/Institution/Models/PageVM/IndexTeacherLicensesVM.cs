using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexTeacherLicensesVM
    {
        public List<OE_UserAuthenticationsListVM> OE_UserAuthenticationsListVM { get; set; }
        //[NOTE: Extra Fields]
        public long Id { get; set; }
        public long ActorId { get; set; }
        public string UserLoginId { get; set; }
        public bool IsActive { get; set; }
    }
}

