using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexViewAttendanceByTeacherVM
    {
        public List<AttendancesListVM> _Atendances { get; set; }

        //[NOTE: Extra Fields from OE_Institution]
        public string InstitutionName { get; set; } //[Note:Orginal name is 'Name']

        //[NOTE: Extra Fields from 'AssignedStudents' Table]
        public long ClassId { get; set; }
        public long StudentId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public long EmployeeId { get; set; }

        //[Extra Field from 'Classes' table]
        public string ClassName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Field from 'Subjects' table]
        public string AssignedCourseName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Field from 'AssignedSections' table]
        public string AssignedSectionName { get; set; } //[Note:Orginal name is 'Name']

        //[NOTE: Extra Fields]
        public long SelectedYear { get; set; }        
    }
}

