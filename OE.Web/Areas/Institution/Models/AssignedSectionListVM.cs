
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class AssignedSectionListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Year { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public long InstitutionId { get; set; }
        public DateTime? AddedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DataType { get; set; }
    }
}
