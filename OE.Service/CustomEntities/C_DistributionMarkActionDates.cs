using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_DistributionMarkActionDates
    {
        public long Id { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool? IsActive { get; set; }
        public long AddedBy { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
