
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class ExamTypeListVM
    {       
        public Int64 Id { get; set; }
        public Int64 InstitutionId { get; set; }
        public string Name { get; set; }
        public Int64 InsId { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DataType { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public int BreakDownInP { get; set; }
        public bool IsLastExam { get; set; }
        public long Sorting { get; set; }
        public bool IsActive { get; set; }
    }
}
