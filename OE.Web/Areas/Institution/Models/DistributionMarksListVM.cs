
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class DistributionMarksListVM
    {
        public Int64 Id { get; set; }
        public long DistributionMarkActionDateId { get; set; }
        public Int64 SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Int64 MarkTypeId { get; set; }
        public string MarkTypeName { get; set; }
        public long BreakDownInP { get; set; }
        public long InstitutionId { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }

    }
}
