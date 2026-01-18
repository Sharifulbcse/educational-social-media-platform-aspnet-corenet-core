
using System;

namespace OE.Data
{
    public class InsCategories : BaseEntity
    {
        public string Name { get; set; }
        public Int64 CountryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
