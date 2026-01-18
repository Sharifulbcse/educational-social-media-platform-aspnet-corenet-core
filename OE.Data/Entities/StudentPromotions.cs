using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class StudentPromotions : BaseEntity
    {
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public long RollNo { get; set; }
        public DateTime Year { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }
    }
}
