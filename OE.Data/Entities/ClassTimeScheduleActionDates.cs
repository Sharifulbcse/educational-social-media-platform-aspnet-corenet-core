using System;

namespace OE.Data
{
    public class ClassTimeScheduleActionDates : BaseEntity
    {
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public long Sorting { get; set; }
        public bool? IsActive { get; set; }

    }
}
