
using Microsoft.AspNetCore.Http;
using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_EmployeeDetails
    {
        public Int64 Id { get; set; }
        public long InstitutionId { get; set; }
        public long EmployeeId { get; set; }
        public string StringValue { get; set; }
        public long? WholeValue { get; set; }
        public double? FloatValue { get; set; }
        public DateTime? DateValue { get; set; }
        public string FilePathValue { get; set; }
        public string ImagePathValue { get; set; }
        public bool? BitValue { get; set; }
        public string TextAreaValue { get; set; }
        public bool? IsActive { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public string DataType { get; set; }
        public long InsId { get; set; }

        //[Extra fields from Registration Group]
        public long? RegistrationGroupId { get; set; }

        //[Extra Fields from Registration ItemType]
        public long RegistrationItemTypeId { get; set; }

        //[Extra Fields from Registration Item]
        public long RegistrationItemId { get; set; }
        public string RegistrationItemName { get; set; }



        public IFormFile ActualFile { get; set; }
        public IFormFile ActualImage { get; set; }

    }
}
