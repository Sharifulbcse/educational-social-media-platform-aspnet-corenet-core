
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class OE_UserAuthenticationsListVM
    {
        public Int64 Id { get; set; }
        public Int64 ActorId { get; set; }
        public string ActorName { get; set; }
        public Int64 InstitutionId { get; set; }
        public Int64 UserId { get; set; }
        public string UserLoginId { get; set; }
        
        public string UserIP300X200 { get; set; }
        public bool? IsActive { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DataType { get; set; }

        //[NOTE: Extra Properties from 'Genders' entity]
        public Int64 GenderId { get; set; }

        //[NOTE: Extra Properties from 'Employees' entity]
        public string EmployeeName { get; set; }

        //[NOTE: Extra Properties from 'OE_Users' entity]
        public string UserName { get; set; }
    }

}
