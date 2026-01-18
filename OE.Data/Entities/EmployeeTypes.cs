
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class EmployeeTypes : BaseEntity
    {
        public long InstitutionId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
