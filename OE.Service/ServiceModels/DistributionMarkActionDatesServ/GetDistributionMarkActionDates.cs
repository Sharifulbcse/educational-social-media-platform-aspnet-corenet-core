using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetDistributionMarkActionDates
    {
        public List<C_DistributionMarkActionDates> DistributionMarkActionDates { get; set; }

        //[NOTE: Extra Fields]
        public string InstitutionName { get; set; }
    }
}

