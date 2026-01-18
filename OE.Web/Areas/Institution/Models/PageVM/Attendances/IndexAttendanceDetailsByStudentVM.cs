using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAttendanceDetailsByStudentVM
    {
        public List<AttendancesListVM> _Atendances { get; set; }
        public AttendancesListVM Attendance { get; set; }

        //[Extra Fields from 'AssignedStudents' Table]
        public long ClassId { get; set; }
        public long StudentId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public long EmployeeId { get; set; }


        //[Extra Fields from 'Students' Table]
        public string StudentName { get; set; } //[Note:Orginal name is 'Name']
        public string IP300X200 { get; set; }

        //[Extra Fields from OE_Institution]
        public string InstitutionName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Fields from Employees]
        public string EmployeeName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Field from 'Classes' table]
        public string ClassName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Field from 'Subjects' table]
        public string AssignedCourseName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Field from 'AssignedSections' table]
        public string AssignedSectionName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Field from 'AttendanceCalculations' table]
        public int TotalClasses { get; set; }
        public int TotalPresents { get; set; }

        //[Extra Fields]
        public int TotalAbsents { get; set; }

    }
}

