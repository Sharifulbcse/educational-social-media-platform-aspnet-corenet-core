
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class ResultsListVM
    {
        public Int64 Id { get; set; }
        public Int64 StudentId { get; set; }
        public string StudentName { get; set; }
        public long InstitutionId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }       
        public Int64 ClassId { get; set; }
        public string ClassName { get; set; }
        public Int64 ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public Int64 SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Int64 MarkTypeId { get; set; }
        public string MarkTypeName { get; set; }
        public Int64 Mark { get; set; }
        public DateTime Year { get; set; }

        //[NOTE: extra field from grade entity]
        public long StartMark { get; set; }
        public long EndMark { get; set; }
        public string Grade { get; set; }
        public Double GPA { get; set; }
        public Double GPAOutOf { get; set; }
    }
}
