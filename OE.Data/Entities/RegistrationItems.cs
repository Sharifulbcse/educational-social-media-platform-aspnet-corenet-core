using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class RegistrationItems : BaseEntity
    {
        public string Name { get; set; }
        public long InstitutionId { get; set; }
        public long RegistrationGroupId { get; set; }
        public long RegistrationItemTypeId { get; set; }
        public long RegistrationUserTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
