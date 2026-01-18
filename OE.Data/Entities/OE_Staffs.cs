
using System;

namespace OE.Data
{
    public class OE_Staffs:BaseEntity 
    {
        public Int64 StaffTypeId { get; set; }      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IP300X200 { get; set; }
        public string IP600X400 { get; set; }

        public string Email { get; set; }
        public string Designation { get; set; }

        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Contact { get; set; }
        public bool? IsActive { get; set; }
    }
}
