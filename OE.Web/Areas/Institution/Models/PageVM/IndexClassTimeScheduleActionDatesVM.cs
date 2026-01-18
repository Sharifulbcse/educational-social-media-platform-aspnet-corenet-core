
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexClassTimeScheduleActionDatesVM
    {
        public List<ClassTimeScheduleActionDatesListVM> _ClassTimeScheduleActionDates { get; set; }
        public ClassTimeScheduleActionDatesListVM ClassTimeScheduleActionDate { get; set; }

        //[NOTE:Extra Field]
        public string InstitutionName { get; set; }
    }
}
