using System;
using System.ComponentModel.DataAnnotations;


namespace OE.Web.Areas.Institution.Models
{
    public class OELicensesListVM
    {
        public Int64 Id { get; set; }
        public Int64 InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public Int64 LicenseNumber { get; set; }
        public string DP { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public Int64 UserId { get; set; }
        public string UserName { get; set; }

    }
}
