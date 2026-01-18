using OE.Data;
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetDistributionMarkList
    {
        public List<C_DistributionMarks> DistributionMark { get; set; }

        //[NOTE: Extra field from 'C_MarkTypes' entity]
        public List<C_MarkTypes> MarkType { get; set; }

        //[NOTE:Extra field from 'C_subject' entity]
        public List<C_Subjects> Subject { get; set; }

        //[NOTE: Extra Fields]
        public string InstitutionName { get; set; }
        public string ClassName { get; set; }
    }
}
