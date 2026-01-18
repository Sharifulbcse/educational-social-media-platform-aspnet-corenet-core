using System;
namespace OE.Data
{
    public class OE_Institutions : BaseEntity
    {
        public string Name { get; set; }
        public Int64 CountryId { get; set; }
        public string LogoPath { get; set; }
        public string FaviconPath { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
    }
}
