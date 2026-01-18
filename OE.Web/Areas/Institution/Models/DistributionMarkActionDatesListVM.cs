using System;
namespace OE.Web.Areas.Institution.Models
{
    public class DistributionMarkActionDatesListVM
    {
        public Int64 Id { get; set; }        
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
