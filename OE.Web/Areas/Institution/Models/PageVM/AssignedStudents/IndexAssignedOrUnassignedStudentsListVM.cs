using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAssignedOrUnassignedStudentsListVM
    {       
        public string InstitutionName { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public long AssignedCourseId { get; set; }
        public string AssignedCourseName { get; set; }
        public long AssignedSectionId { get; set; }
        public string AssignedSectionName { get; set; }
        public long Year { get; set; }        
        public long SubjectTypeId { get; set; }
        public long StudentId { get; set; }
        public bool IsAssigned { get; set; }
        public AssignedStudentsListVM AssignedStudents { get; set; }
        public List<StudentPromotionsListVM> _StudentPromotions { get; set; }
        public List<AssignedStudentsListVM> _AssignedStudents { get; set; }
    }
}

