using System;

namespace OE.Web.Areas.Institution.Models
{
    public class AssignedStudentsListVM
    {
        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public long AssignedCourseId { get; set; }
        public string AssignedCourseName { get; set; }
        public long? AssignedSectionId { get; set; }
        public string AssignedSectionName { get; set; }
        public long? SubjectTypeId { get; set; }
        public long InsId { get; set; }
        public DateTime Year { get; set; }

        //[NOTE: Extra Fields from 'Students' table]
        public string IP300X200 { get; set; }

        //[NOTE: Extra Fields]
        public bool IsAssigned { get; set; }
    }
}

