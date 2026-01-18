using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_Attendances
    {
        public long Id { get; set; }
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

        //[NOTE: Extra Fields from 'Students' Table]
        public string IP300X200 { get; set; }
        public string StudentName { get; set; }

        //[NOTE: Extra Fields]
        public int TotalClasses { get; set; }
        public int TotalPresents { get; set; }
        public int TotalAbsents { get; set; }

        //[NOTE: Extra Fields from 'Students' Table]
        public string TimeSlot { get; set; }
    }
}

