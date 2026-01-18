using System;
using Microsoft.AspNetCore.Http; //[NOTE: for IFormFile]

namespace OE.Web.Areas.Institution.Models
{
    public class StudentsDetailsListVM
    {
        public Int64 Id { get; set; }
        public long StudentId { get; set; }
        public long InstitutionId { get; set; }
        public long? RegistrationGroupId { get; set; }
        public long RegistrationItemTypeId { get; set; }

        public long RegistrationItemId { get; set; }
        public string RegistrationItemName { get; set; }

        public string StringValue { get; set; }
        public long? WholeValue { get; set; }
        public double? FloatValue { get; set; }
        public DateTime? DateValue { get; set; }
        public string FilePathValue { get; set; }
        public string ImagePathValue { get; set; }
        public bool? BitValue { get; set; }
        public string TextAreaValue { get; set; }

        
        public IFormFile ActualFile { get; set; }
        public IFormFile ActualImage { get; set; }
        public bool? IsActive { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public string DataType { get; set; }
        
    }
}
