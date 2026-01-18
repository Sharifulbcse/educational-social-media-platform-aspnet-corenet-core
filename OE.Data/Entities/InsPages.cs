using System;

namespace OE.Data
{
    public class InsPages : BaseEntity
    {
        public string Title { get; set; }
        public string IP300X200 { get; set; }
        public string IP600X400 { get; set; }
        public bool? IsActive { get; set; }
    }
}
