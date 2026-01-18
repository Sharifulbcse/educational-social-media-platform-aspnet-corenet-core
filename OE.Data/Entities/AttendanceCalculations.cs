
using System;

namespace OE.Data
{
    public class AttendanceCalculations : BaseEntity
    {
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public int TotalAttendance { get; set; }
        public int TotalClass { get; set; }
        public DateTime Year { get; set; }
        public long InsId { get; set; }
    }
}


