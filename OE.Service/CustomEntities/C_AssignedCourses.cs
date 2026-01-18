using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_AssignedCourses
    {
        public long Id { get; set; }
        public DateTime Year { get; set; }
        public long ClassId { get; set; }
        public long AssignedSectionId { get; set; }
        public long SubjectId { get; set; }
        public long InstitutionId { get; set; }
        public bool? IsActive { get; set; }

        //[NOTE: Field from 'Subject' table]
        public string SubjectName { get; set; }
    }
}

