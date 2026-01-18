using System;

namespace OE.Web.Areas.Institution.Models
{
    public class AssignedTeachersListVM
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public long EmployeeId { get; set; }
        public long InstitutionId { get; set; }

        public DateTime Year { get; set; }       
        public string ClassName { get; set; }
       
        public string AssignedCourseName { get; set; }        
        public string AssignedSectionName { get; set; }
        public string EmployeeName { get; set; }        
        
        public long InsId { get; set; }
        public string IP300X200 { get; set; }
       
    }
}

