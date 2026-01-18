using System;

namespace OE.Data
{
    public class GradeTypes : BaseEntity
    {
        public Int64 InstitutionId { get; set; }
        public Int64 StartMark { get; set; }
        public Int64 EndMark { get; set; }
        public string Grade { get; set; }        
        public Double GPA { get; set; }
        public Double GPAOutOf { get; set; }
       
        public long ClassId { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }
    }
}
