
using System;

namespace OE.Data
{
    public class Employees : BaseEntity
    {
        public long GenderId { get; set; }
        public long? UserId { get; set; }
        public long InstitutionId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IP300X200 { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DOB { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool? IsActive { get; set; }

        public long InsId { get; set; }
    }
}
