using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAttendancesVM
    {
        public List<AttendancesListVM> _Atendances { get; set; }
        
        public List<AssignedTeachersListVM> _AssignTeachers { get; set; }
        public List<AssignedStudentsListVM> _AssignedStudents { get; set; }

        //[Extra Fields from 'AssignedStudents' Table]
        public long ClassId { get; set; }
        public string ClassName { get; set; }

        public long AssignedCourseId { get; set; }
        public string AssignedCourseName { get; set; }
        public long AssignedSectionId { get; set; }
        public string AssignedSectionName { get; set; }

        //[Extra Fields from 'Attendances' Table]
        public long ClassTimeScheduleId { get; set; }
        public DateTime SelectedDate { get; set; }

        //[Extra Fields from 'Student' Table]
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public string IP300X200 { get; set; }
       
        //[Extra Fields from OE_Institution]
        public string InstitutionName { get; set; }

        //[Extra Fields from Employees]
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        //[Extra Fields]
        public long CurrentYear { get; set; }
        public long SelectedYear { get; set; }
        public int TotalClasses { get; set; }
        public int TotalPresents { get; set; }
        public int TotalAbsents { get; set; }
    }
}

