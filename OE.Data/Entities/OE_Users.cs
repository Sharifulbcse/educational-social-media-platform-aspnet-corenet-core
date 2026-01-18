
using System;

namespace OE.Data
{
    public class OE_Users : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IP300X200 { get; set; }
        public string IP600X400 { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Int64 GenderId { get; set; }
        public string UserLoginId { get; set; } 
        public string Password { get; set; }
        public bool IsForgetPassword { get; set; }
        public bool? IsActive { get; set; }
    }
}
