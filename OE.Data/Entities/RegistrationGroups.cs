
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class RegistrationGroups : BaseEntity
    {
        public string Name { get; set; }
        public long InstitutionId { get; set; }
        public long RegistrationUserTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
