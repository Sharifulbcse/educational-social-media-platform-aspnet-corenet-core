using System;

namespace OE.Web.Areas.Institution.Models
{
    public class ClassTimeSchedulesListVM
    {

        public long Id { get; set; }
        public long ClassTimeScheduleActionDateId { get; set; }       
        public long Sorting { get; set; }        
        public long InstitutionId { get; set; }
        public TimeSpan ClassStartTime { get; set; }
        public TimeSpan ClassEndTime { get; set; }
        public long InsId { get; set; }
       
        //[NOTE: Extra Field]
        public string ClassStartTimeSlot { get; set; }
        public string ClassEndTimeSlot { get; set; }
        

    }
}

