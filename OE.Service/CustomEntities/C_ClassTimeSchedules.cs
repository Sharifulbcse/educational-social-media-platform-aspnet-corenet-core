using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_ClassTimeSchedules
    {
        public long Id { get; set; }
        
        public long ClassTimeScheduleActionDateId { get; set; }
        
        public long InstitutionId { get; set; }
        public TimeSpan ClassStartTime { get; set; }
        public TimeSpan ClassEndTime { get; set; }
       
        public long InsId { get; set; }
        public long Sorting { get; set; }
        public bool IsActive { get; set; }

        //[NOTE: Extra Fields]
        public string TimeStot { get; set; }
        public string ClassStartTimeSlot { get; set; }
        public string ClassEndTimeSlot { get; set; }

    }
}

