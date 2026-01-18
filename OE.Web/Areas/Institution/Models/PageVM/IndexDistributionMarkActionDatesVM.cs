using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexDistributionMarkActionDatesVM
    {
        public List<DistributionMarkActionDatesListVM> distributionMarkActionDates { get; set; }
        public DistributionMarkActionDatesListVM DistributionMarkActionDate { get; set; }

        //[NOTE:Extra Field]
        public string InstitutionName { get; set; }
    }
}
