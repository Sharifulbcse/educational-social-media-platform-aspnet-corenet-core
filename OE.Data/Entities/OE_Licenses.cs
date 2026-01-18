using System;

namespace OE.Data
{
    public class OE_Licenses : BaseEntity
    {
        public Int64 InstitutionId { get; set; }
        public Int64 LicenseNumber { get; set; }
        public string DP { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
