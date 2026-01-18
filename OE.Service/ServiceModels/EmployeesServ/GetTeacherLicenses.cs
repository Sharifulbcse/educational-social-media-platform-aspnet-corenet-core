using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetTeacherLicenses
    {
        public OE_UserAuthentications OE_UserAuthentications { get; set; }
        public OE_Actors OE_Actors { get; set; }
        public Employees Employees { get; set; }
        public OE_Users OE_Users { get; set; }
    }
}

