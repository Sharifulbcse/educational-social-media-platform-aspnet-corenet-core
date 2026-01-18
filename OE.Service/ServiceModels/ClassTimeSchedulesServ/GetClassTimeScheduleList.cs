using OE.Data;
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetClassTimeScheduleList
    {
        public List<C_ClassTimeSchedules> _C_ClassTimeSchedule { get; set; }

        //[NOTE: Extra Fields]
        public string InstitutionName { get; set; }
    }
}
