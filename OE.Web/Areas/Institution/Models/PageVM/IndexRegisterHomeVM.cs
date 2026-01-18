using System;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexRegisterHomeVM
    {
        public Int64 Id { get; set; }
        public Int64? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
    }
}