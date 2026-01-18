
using System;

namespace OE.Data
{
    public class DistributionMarks : BaseEntity
    {
        public Int64 SubjectId { get; set; }
        public long DistributionMarkActionDateId { get; set; }
        public long InstitutionId { get; set; }
        public Int64 MarkTypeId { get; set; }
        public long ClassId { get; set; }
        public long InsId { get; set; }       
        public long BreakDownInP { get; set; }
        
        public bool? IsActive { get; set; }

    }
}
