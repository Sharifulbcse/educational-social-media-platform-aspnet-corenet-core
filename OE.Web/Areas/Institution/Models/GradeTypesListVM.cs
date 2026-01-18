
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class GradeTypesListVM
    {    
        public Int64 Id { get; set; }
        public Int64 InstitutionId { get; set; }
        public Int64 StartMark { get; set; }
        public Int64 EndMark { get; set; }
        public string Grade { get; set; }
        public Double GPA { get; set; }
        public Double GPAOutOf { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }

    }
}
