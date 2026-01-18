using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexClassTimeSchedulesVM
    {
        public List<ClassTimeSchedulesListVM> _ClassTimeSchedules { get; set; }
        public ClassTimeSchedulesListVM ClassTimeSchedules { get; set; }
        public long ClassTimeScheduleActionDateId { get; set; }
        
        //[NOTE: Extra properties]
        public string InstitutionName { get; set; }
        
    }
}

