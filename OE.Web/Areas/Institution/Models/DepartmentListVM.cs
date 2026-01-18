
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class DepartmentListVM
    {
       public Int64 Id { get; set; }
        public string Name { get; set; }
        public long InstitutionId { get; set; }
        public Int64 InsId { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DataType { get; set; }
    }
}
