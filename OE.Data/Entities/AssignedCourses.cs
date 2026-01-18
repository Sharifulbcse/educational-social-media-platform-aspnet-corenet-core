
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class AssignedCourses : BaseEntity
    {
        public DateTime Year { get; set; }
        public long ClassId { get; set; }
        public long AssignedSectionId { get; set; }
        public long SubjectId { get; set; }
        public long InstitutionId { get; set; }
        public bool? IsActive { get; set; }
    }
}

