
using System;

namespace OE.Data
{
    public class DistributionMarkActionDates : BaseEntity
    {
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool? IsActive { get; set; }

    }
}
