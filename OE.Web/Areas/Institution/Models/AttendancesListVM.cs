using System;

namespace OE.Web.Areas.Institution.Models
{
    public class AttendancesListVM
    {
        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long ClassTimeScheduleId { get; set; }
        public long EmployeeId { get; set; }
        public long AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }


        //[NOTE: Extra Fields from 'Students' Table]
        public string StudentName { get; set; }
        public string IP300X200 { get; set; }
       
        //[NOTE: Extra Fields]
        public string TimeSlot { get; set; }
        public int TotalClasses { get; set; }
        public int TotalPresents { get; set; }
        public int TotalAbsents { get; set; }
        public long AssignedCourseId { get; set; }
        public long ClassId { get; set; }
        public long AssignedSectionId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        public long InsId { get; set; }


        


    }
}
