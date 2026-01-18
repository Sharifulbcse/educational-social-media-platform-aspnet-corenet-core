
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class StudentPromotionsListVM
    {
        public Int64 Id { get; set; }
        public long RollNo { get; set; }
        public long PromotionYear { get; set; }
       
        public DateTime Year { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        
        public long InstitutionId { get; set; }
        public long PromotionClassId { get; set; }
        public string PromotionClassName { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentPhoto { get; set; }
        
        //[NOTE: Extra Fields]
        public long UserId { get; set; }
        public string UserLoginId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllChecked { get; set; }
        public bool IsAssigned { get; set; }

    }
}



