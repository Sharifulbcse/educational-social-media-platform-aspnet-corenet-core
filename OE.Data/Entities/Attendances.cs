using System;

namespace OE.Data
{
    public class Attendances : BaseEntity
    {
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long ClassTimeScheduleId { get; set; }
        public long EmployeeId { get; set; }
        public long AssignedCourseId { get; set; }
        public long ClassId { get; set; }
        public long AssignedSectionId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        public long InsId { get; set; }
    }
}

