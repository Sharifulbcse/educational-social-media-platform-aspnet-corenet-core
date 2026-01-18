
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class InsCategoryListVM
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public Int64 CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
