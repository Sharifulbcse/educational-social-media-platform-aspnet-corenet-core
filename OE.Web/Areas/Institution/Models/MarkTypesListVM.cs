
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class MarkTypesListVM
    {
        public Int64 Id { get; set; }
        public long InstitutionId { get; set; }
        public string Name { get; set; }
        public long BreakDownInP { get; set; }
        public bool? IsActive { get; set; }       
       
        
    }
}
