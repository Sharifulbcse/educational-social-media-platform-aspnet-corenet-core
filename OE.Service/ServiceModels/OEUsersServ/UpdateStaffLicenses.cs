using OE.Data;

namespace OE.Service.ServiceModels
{
    public class UpdateStaffLicenses
    {
        public OE_UserAuthentications OE_UserAuthentications { get; set; }
        //[NOTE: Extra]
        public string SelectedUserLoginId { get; set; }
        public long SelectedActorId { get; set; }
        public string Message { get; set; }
    }
}

