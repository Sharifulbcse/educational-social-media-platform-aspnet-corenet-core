using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetClassTimeScheduleActionDates
    {
        public List<C_ClassTimeScheduleActionDates> _C_ClassTimeScheduleActionDates { get; set; }

        //[NOTE: Extra Fields]
        public string InstitutionName { get; set; }
    }
}
