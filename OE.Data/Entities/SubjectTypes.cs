
using System;

namespace OE.Data
{
    public class SubjectTypes : BaseEntity
    {
        public string Name { get; set; }
        public Int64 InstitutionId { get; set; }
        public bool? IsActive { get; set; }
    }
}
