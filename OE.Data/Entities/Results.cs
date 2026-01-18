
using System;

namespace OE.Data
{
    public class Results : BaseEntity
    {
        public Int64 StudentId { get; set; }
        public long InstitutionId { get; set; }
        public long EmployeeId { get; set; }
        public Int64 ClassId { get; set; }
        public Int64 ExamTypeId { get; set; }
        public Int64 SubjectId { get; set; }
        public Int64 MarkTypeId { get; set; }
        public Int64 Mark { get; set; }
        public DateTime Year { get; set; }
        public bool? IsActive { get; set; }
    }
}
