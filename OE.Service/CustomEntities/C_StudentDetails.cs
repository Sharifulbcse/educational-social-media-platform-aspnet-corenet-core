using Microsoft.AspNetCore.Http;
using System;

namespace OE.Service.CustomEntitiesServ
{
    public class C_StudentDetails
    {
        public long Id { get; set; }
        public long InstitutionId { get; set; }
        public long StudentId { get; set; }
        public long RegistrationItemId { get; set; }
        public string StringValue { get; set; }
        public long? WholeValue { get; set; }
        public double? FloatValue { get; set; }
        public DateTime? DateValue { get; set; }
        public string FilePathValue { get; set; }
        public string ImagePathValue { get; set; }
        public bool? BitValue { get; set; }
        public string TextAreaValue { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }
        public long AddedBy { get; set; }
        
        //[NOTE: Fields from 'COM_RegistrationGroups' entity]
        public long RegistrationGroupId { get; set; }

        //[NOTE: Fields from 'COM_RegistrationItemTypes' entity]
        public long RegistrationItemTypeId { get; set; }

        //[NOTE: Extra Fields]
        public IFormFile ActualFile { get; set; }
        public IFormFile ActualImage { get; set; }        
        public string RegistrationItemName { get; set; }
       
    }
}

