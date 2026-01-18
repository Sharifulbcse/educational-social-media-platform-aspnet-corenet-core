using System;

namespace OE.Web.Areas.Institution.Models
{
    public class EmployeeTypeCategoriesListVM
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public long InstitutionId { get; set; }
        public long EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public Int64 InsId { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DataType { get; set; }
    }
}

