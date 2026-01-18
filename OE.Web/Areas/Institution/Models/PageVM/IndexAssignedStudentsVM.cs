using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAssignedStudentsVM
    {
        public List<AssignedStudentsListVM> _AssignedStudents { get; set; }      
        public List<AssignedStudentsListVM> _StudentsPromotion { get; set; }        
        public ClassesListVM Classes { get; set; }
        public AssignedCoursesListVM AssignedCourses { get; set; }
        public AssignedSectionListVM AssignedSection { get; set; }

        //[NOTE: Extra Fields]
        public int Year { get; set; }
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public string InstitutionName { get; set; }
        

    }
}
