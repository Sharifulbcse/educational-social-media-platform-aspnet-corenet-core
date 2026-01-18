using System;

namespace OE.Web.Areas.Institution.Models
{
    public class AssignedCoursesListVM
    {
        public long Id { get; set; }
        public DateTime Year { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public long AssignedSectionId { get; set; }
        public string SectionName { get; set; }
        public long SubjectId { get; set; }
        public string CourseName { get; set; }
        public long InstitutionId { get; set; }

        //[NOTE: Extra Fields from table 'Subjects']
        public long SubjectTypeId { get; set; }

    }
}
