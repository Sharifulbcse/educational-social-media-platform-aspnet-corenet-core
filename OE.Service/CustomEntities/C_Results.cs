using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_Results
    {
        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long EmployeeId { get; set; }
        public long ClassId { get; set; }
        public long ExamTypeId { get; set; }
        public long MarkTypeId { get; set; }
        public long SubjectId { get; set; }
        public long Mark { get; set; }
        public DateTime Year { get; set; }
        //[NOTE:Extra field from Grade Type entity]
        public long StartMark { get; set; }
        public long EndMark { get; set; }
        public string Grade { get; set; }
        public Double GPA { get; set; }
        public Double GPAOutOf { get; set; }
        //[NOTE:Extra field for Subject entity]
        public string SubjectName { get; set; }

        //[NOTE:Extra field for class entity]
        public string ClassName { get; set; }

    }
}

