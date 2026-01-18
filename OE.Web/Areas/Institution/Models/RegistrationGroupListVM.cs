
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class RegistrationGroupListVM
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public long InstitutionId { get; set; }
        public long RegistrationUserTypeId { get; set; }
        public string RegistrationUserTypeName { get; set; }

    }
}
