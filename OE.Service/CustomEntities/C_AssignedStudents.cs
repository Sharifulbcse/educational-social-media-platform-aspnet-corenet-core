using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_AssignedStudents
    {

        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }
        public long? AssignedSectionId { get; set; }
        public long? SubjectTypeId { get; set; }
        public long InsId { get; set; }
        public DateTime Year { get; set; }

        //[NOTE:Extra Field from 'AssignedSections' table]
        public string SectionName { get; set; }//[NOTE:Orginal name is 'Name']

        //[NOTE:Extra Field from 'Classes' table]
        public string ClassName { get; set; }//[NOTE:Orginal name is 'Name']

        //[NOTE: Fields from 'GradeTypes' entity]
        public string Grade { get; set; }

        //[NOTE: Fields from 'Students' entity]
        public string StudentName { get; set; }
        public string IP300X200 { get; set; }

        //[NOTE: Fields from 'Subject' entity]
        public string SubjectName { get; set; }              

        //[NOTE: Extra Fields]
        public long TotalMark { get; set; }
        public bool IsAssigned { get; set; }
        public long AddedBy { get; set; }


    }
}

