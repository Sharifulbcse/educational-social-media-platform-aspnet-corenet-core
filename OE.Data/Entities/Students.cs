using System;
using System.ComponentModel.DataAnnotations;
namespace OE.Data
{
    public class Students : BaseEntity
    {        
        public Int64 InstitutionId { get; set; }
        
        public Int64 ClassId { get; set; }
        public Int64 GenderId { get; set; }
        public string Name { get; set; }
        public string IP300X200 { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }       
        public Int64? UserId { get; set; }
        public DateTime DOB { get; set; }
        public DateTime AdmittedYear { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }

    }
}

