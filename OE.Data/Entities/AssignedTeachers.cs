
using System;

namespace OE.Data
{
    public class AssignedTeachers : BaseEntity
    {
        public DateTime Year { get; set; }
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
       
        public long EmployeeId { get; set; }
        public long InstitutionId { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }
    }
}

