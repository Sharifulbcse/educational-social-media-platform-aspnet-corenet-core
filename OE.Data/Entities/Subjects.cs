
using System;

namespace OE.Data
{
    public class Subjects : BaseEntity
    {
        public string Name { get; set; }
        public long InstitutionId { get; set; }        
        public Int64 ClassId { get; set; }        
        public Int64 SubjectTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
