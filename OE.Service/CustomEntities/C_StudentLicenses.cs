
namespace OE.Service.CustomEntitiesServ
{
    public class C_StudentLicenses
    {
        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long? UserId { get; set; }
        public long AddedBy { get; set; }
        public long ModifiedBy { get; set; }

        //[NOTE: Fields from 'OE_UserAuthentications' entity]
        public bool? IsActive { get; set; }
        public bool? oldIsActive { get; set; }

        //[NOTE: Fields from 'OE_Users' entity]
        public string UserLoginId { get; set; }
        public string oldUserLoginId { get; set; }

        //[NOTE: Extra Fields only]
        public string Message { get; set; }
    }
}

