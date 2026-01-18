
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class OE_InstitutionsListVM
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public Int64 CountryId { get; set; }       
        public string CountryName { get; set; }
        public bool IsLicense { get; set; }
        public bool? IsActive { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DataType { get; set; }


        public string LogoPath { get; set; }
        public string FaviconPath { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
    }
}
