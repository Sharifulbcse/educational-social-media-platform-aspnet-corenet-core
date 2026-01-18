namespace OE.Service.ServiceModels
{
    public class UpdateTeacherLicenses
    {
        //[NOTE: Extra]
        public long Id { get; set; }
        public long ActorId { get; set; }
        public string UserLoginId { get; set; }
        public long InstitutionId { get; set; }
        public long ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
    }
}


