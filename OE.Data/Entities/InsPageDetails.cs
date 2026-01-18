using System;

namespace OE.Data
{
    public class InsPageDetails : BaseEntity
    {
        public long InsPageId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public long Sorting { get; set; }
        public bool? IsActive { get; set; }
    }
}
