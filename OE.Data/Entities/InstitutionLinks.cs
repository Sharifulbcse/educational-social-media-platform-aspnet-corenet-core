using System;

namespace OE.Data
{
    public class InstitutionLinks : BaseEntity
    {
        public string Name { get; set; }
        public Int64 InstitutionId { get; set; }
        public string Url { get; set; }
        public string IP24X24 { get; set; }
        public bool? IsActive { get; set; }
    }
}
