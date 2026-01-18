
using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexPrintSubjectListVM
    {
        public List<AssignedStudentsListVM> _AssignStudents { get; set; }

        //[NOTE: Extra Fields from 'AssignedStudents' Table]
        public long ClassId { get; set; }
        public long StudentId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }

        //[NOTE: Extra Fields from OE_Institution]
        public string InstitutionName { get; set; } //[Note:Orginal name is 'Name']

        //[NOTE: Extra Fields from Students table]
        public string StudentName { get; set; } //[Note:Orginal name is 'Name']

        //[NOTE: Extra Fields]
        public long SelectedYear { get; set; }

    }
}
