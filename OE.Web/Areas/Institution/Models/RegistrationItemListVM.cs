
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class RegistrationItemListVM
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public long InstitutionId { get; set; }
        public long RegistrationGroupId { get; set; }
        public string RegistrationGroupName { get; set; }
        public long RegistrationItemTypeId { get; set; }
        public string RegistrationItemTypeName { get; set; }
        public long RegistrationUserTypeId { get; set; }
    }
}
