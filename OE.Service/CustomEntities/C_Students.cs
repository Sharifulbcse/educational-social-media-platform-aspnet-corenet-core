
using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_Students
    {
        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long ClassId { get; set; }
        public long GenderId { get; set; }
        public string Name { get; set; }
        public string IP300X200 { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public long? UserId { get; set; }
        public DateTime DOB { get; set; }
        public DateTime AdmittedYear { get; set; }
        public long InsId { get; set; }

        //[NOTE: Fields from 'Classes' entity]
        public string ClassName { get; set; }

        //[NOTE:Extra Fields from Gender entity]
        public string GenderName { get; set; }

        //[NOTE: Fields from 'OE_UserAuthentications' entity]
        public bool? IsActive { get; set; }

        //[NOTE: Fields from 'OE_Users' entity]
        public string UserLoginId { get; set; }
        public string UsersIP300X200 { get; set; }

        //[NOTE: Fields from 'StudentPromotions' entity]
        public long StudentPromotionClassId { get; set; }
        public DateTime StudentPromotionYear { get; set; }
        public long RollNo { get; set; }
        public long AdmittedClassId { get; set; }
        public long AdmittedPYear { get; set; }

        //[NOTE: Extra Fields only]
        public string OldUserLoginId { get; set; }
        public bool IsLicense { get; set; }
        public string Message { get; set; }
    }
}

