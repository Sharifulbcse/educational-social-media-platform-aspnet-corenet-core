using System;
using Microsoft.AspNetCore.Http; //[NOTE: for IFormFile]
namespace OE.Web.Areas.Institution.Models
{
    public class StudentsListVM
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime AdmittedYear { get; set; }
        public DateTime DOB { get; set; }
        public bool? IsActive { get; set; }

        public Int64 InstitutionId { get; set; }        
        public Int64 ClassId { get; set; }
        public string ClassName { get; set; }
        public Int64 GenderId { get; set; }
        public string GenderName { get; set; }
        public string Name { get; set; }
        public string IP300X200 { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public bool EnableAuthentic { get; set; }
        public IFormFile ActualFile { get; set; }
               
        //[NOTE: Extra Fields from 'StudentPromotions' entity]
        public long StudentPromotionClassId { get; set; }
        public DateTime StudentPromotionYear { get; set; }
        public long RollNo { get; set; }

        //[NOTE: Extra Fields from 'OE_Users' entity]
        public string UserLoginId { get; set; }
        public string oldUserLoginId { get; set; }
        public string UsersIP300X200 { get; set; }

        //[NOTE:Extra field for license]
        public bool? oldIsActive { get; set; }
        public string message { get; set; }
    }
}
