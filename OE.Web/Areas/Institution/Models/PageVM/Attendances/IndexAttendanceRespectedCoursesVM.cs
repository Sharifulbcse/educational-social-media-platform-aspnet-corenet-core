using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAttendanceRespectedCoursesVM
    {
        public List<AssignedTeachersListVM> _AssignTeachers { get; set; }

        //[Extra Fields from 'AssignedStudents' Table]
        public long ClassId { get; set; }
        public long StudentId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public long EmployeeId { get; set; }

        //[Extra Fields from OE_Institution]
        public string InstitutionName { get; set; } //[Note:Orginal name is 'Name']

        //[Extra Fields]
        public long SelectedYear { get; set; }

    }
}

