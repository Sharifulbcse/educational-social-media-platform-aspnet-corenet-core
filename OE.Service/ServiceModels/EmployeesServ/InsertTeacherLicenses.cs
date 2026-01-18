namespace OE.Service.ServiceModels
{
    public class InsertTeacherLicenses
    {
        //[NOTE: Extra]
        public string UserLoginId { get; set; }
        public long ActorId { get; set; }
        public long InstitutionId { get; set; }
        public long AddedBy { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
    }
}

