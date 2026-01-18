using OE.Data;

namespace OE.Service.ServiceModels
{
    public class InsertStaffLicenses
    {
        public OE_UserAuthentications OE_UserAuthentications { get; set; }
        //[NOTE: Extra]
        public string UserLoginId { get; set; }
        public string Message { get; set; }
    }
}

