
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class ExamTypes : BaseEntity
    {
        public Int64 InstitutionId { get; set; }       
        public string Name { get; set; }
        public long ClassId { get; set; }
        public long InsId { get; set; }
        public int BreakDownInP { get; set; }
        public bool IsLastExam { get; set; }
        public bool? IsActive { get; set; }
        public long Sorting { get; set; }
    }
}
