using System;

namespace OE.Web.Areas.Institution.Models
{
    public class OE_UsersListVM
    {
        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IP300X200 { get; set; }
        public string IP600X400 { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Int64 GenderId { get; set; }        
        public string GenderName { get; set; }
        public string UserLoginId { get; set; }
        public string Password { get; set; }
        public bool IsForgetPassword { get; set; }
        public bool? IsActive { get; set; }
    }
    public class OE_ddlModel
    {
        public int Id { get; set; }
        public string value { get; set; }
    }
}
