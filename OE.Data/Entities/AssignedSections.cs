
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class AssignedSections : BaseEntity
    {
        public string Name { get; set; }
        public DateTime Year { get; set; }
        public Int64 ClassId { get; set; }
        public long InstitutionId { get; set; }
        public bool? IsActive { get; set; }
    }
}

