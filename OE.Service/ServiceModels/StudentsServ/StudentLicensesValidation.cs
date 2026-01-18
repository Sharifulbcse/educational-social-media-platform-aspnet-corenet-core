namespace OE.Service.ServiceModels
{
    public class StudentLicensesValidation
    {
        public long StudentId { get; set; }
        public string SelectedUserLoginId { get; set; }
        public long InstitutionId { get; set; }
        public string Message { get; set; }
    }
}

