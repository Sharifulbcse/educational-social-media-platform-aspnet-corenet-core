using System;

namespace OE.Data
{
    public class EmployeeDesignations : BaseEntity
    {
        public long InstitutionId { get; set; }
        public long EmployeeId { get; set; }
        public long EmployeeTypeId { get; set; }       
        public long? EmployeeTypeCategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }
    }
}
