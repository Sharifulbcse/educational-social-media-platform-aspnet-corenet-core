using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class AssignedStudents : BaseEntity
    {
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }        
        public long? AssignedSectionId { get; set; }
        public long? SubjectTypeId { get; set; }
        public long InsId { get; set; }
        public DateTime Year { get; set; }
        public bool? IsActive { get; set; }

    }
}

