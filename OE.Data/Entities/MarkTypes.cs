
using System;

namespace OE.Data
{
    public class MarkTypes : BaseEntity
    {
        public string Name { get; set; }        
        public long InstitutionId { get; set; }
        public bool? IsActive { get; set; }
    }
}
