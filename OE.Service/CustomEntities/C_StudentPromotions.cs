using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_StudentPromotions
    {
        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public long ClassId { get; set; }
        public long RollNo { get; set; }
        public DateTime Year { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }

        //[NOTE: Extra Fields]
        public bool IsAssigned { get; set; }

    }
}

