
using System;
namespace OE.Web.Areas.Institution.Models
{
    public class ClassTimeScheduleActionDatesListVM
    {
        public Int64 Id { get; set; }
        public long Sorting { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
