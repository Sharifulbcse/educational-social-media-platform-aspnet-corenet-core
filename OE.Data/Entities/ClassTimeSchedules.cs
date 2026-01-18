using System;

namespace OE.Data
{
    public class ClassTimeSchedules : BaseEntity
    {
        public long InstitutionId { get; set; }        
        public long ClassTimeScheduleActionDateId { get; set; }        
        public TimeSpan ClassStartTime { get; set; }
        public TimeSpan ClassEndTime { get; set; }
        public long Sorting { get; set; }
        public long InsId { get; set; }
        public bool IsActive { get; set; }
    }
}

