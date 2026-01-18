using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAttendanceStudentListVM
    {
        public List<AssignedStudentsListVM> _AssignedStudents { get; set; }
        public List<AttendancesListVM> _Atendances { get; set; }
        
        //[NOTE:Extra Fields from 'AssignedStudents' Table]
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }

        //[NOTE:Extra Fields from OE_Institution]
        public string InstitutionName { get; set; } //[Note:Orginal name is 'Name']

        //[NOTE:Extra Fields from 'Attendances' Table]
        public long ClassTimeScheduleId { get; set; }
        public DateTime SelectedDate { get; set; }

        //[Extra Fields]
        public long SelectedYear { get; set; }       
    }
}

