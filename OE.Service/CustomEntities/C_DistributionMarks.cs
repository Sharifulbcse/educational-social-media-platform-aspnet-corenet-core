using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_DistributionMarks
    {

        public long Id { get; set; }
        public long DistributionMarkActionDateId { get; set; }       
        public long SubjectId { get; set; }
        public long MarkTypeId { get; set; }
        public long BreakDownInP { get; set; }
        public long InstitutionId { get; set; }
        public long ClassId { get; set; }
        public long InsId { get; set; }
        public bool IsActive { get; set; }

        //[NOTE: Fields from 'MarkTypes' entity]
        public string MarkTypeName { get; set; }

        //[NOTE:Extra field]
        public string SubjectName { get; set; }
    }
}

