using OE.Data;
using System;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class InsertAttendances
    {
        public List<Attendances> _Attendances { get; set; }
        
        //[NOTE: Extra Fields]
        public long InstitutionId { get; set; }
        public long ClassTimeScheduleId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public long ClassId { get; set; }
        public DateTime SelectedDate { get; set; }

    }
}

