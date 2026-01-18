using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class EmployeeTypeCategories : BaseEntity
    {

        public string Name { get; set; }
        public long InstitutionId { get; set; }
        public long EmployeeTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
